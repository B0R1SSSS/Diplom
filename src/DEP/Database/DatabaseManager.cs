using System;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Npgsql;
using DEP.Models;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace DEP.Database
{
    /// <summary>
    /// Manages database operations
    /// Управляет операциями с базой данных
    /// </summary>
    public class DatabaseManager
    {
        TaskInfo SubmissionStatus;
        private static DatabaseManager instance;
        private string connectionString;

        /// <summary>
        /// Gets the singleton instance of DatabaseManager
        /// Получает единственный экземпляр DatabaseManager
        /// </summary>
        public static DatabaseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DatabaseManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// Private constructor for singleton pattern
        /// Приватный конструктор для паттерна Singleton
        /// </summary>
        private DatabaseManager()
        {
            connectionString = "Server=116.202.229.219;Port=5432;Database=exampapers;User Id=dodep;Password=dodep;";
        }

        /// <summary>
        /// Downloads a submission file to the specified directory
        /// Загружает файл отправленной работы в указанную директорию
        /// </summary>
        /// <param name="submissionId">ID of the submission to download
        /// ID отправленной работы для загрузки</param>
        /// <param name="targetDirectory">Directory where to save the file
        /// Директория для сохранения файла</param>
        /// <returns>Full path to the downloaded file if successful, null otherwise
        /// Полный путь к загруженному файлу в случае успеха, null в противном случае</returns>
        public string DownloadSubmissionToDirectory(int submissionId, string targetDirectory)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Close();
                    connection.Open();
                    using (var command = new NpgsqlCommand(@"
                        SELECT filename, filedata, filetype
                        FROM submissions
                        WHERE submissionid = @submissionId", connection))
                    {
                        command.Parameters.AddWithValue("@submissionId", submissionId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var fileName = reader.GetString(0);
                                var fileData = (byte[])reader.GetValue(1);
                                var fileType = reader.GetString(2);

                                // Создаем директорию, если она не существует
                                Directory.CreateDirectory(targetDirectory);

                                // Формируем полный путь к файлу
                                var fullPath = Path.Combine(targetDirectory, fileName);

                                // Сохраняем файл
                                File.WriteAllBytes(fullPath, fileData);
                                return fullPath;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при скачивании файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        /// <summary>
        /// Gets all submissions
        /// Получает все отправленные работы
        /// </summary>
        /// <returns>List of submissions / Список работ</returns>
        public List<TaskInfo> GetAllSubmissions()
        {
            var submissions = new List<TaskInfo>();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            SELECT s.submissionid, s.taskid, s.studentid, s.submittedat, s.status, s.filename, s.feedback,
                                   t.title as tasktitle, u.fullname as studentname
                            FROM Submissions s
                            JOIN Tasks t ON s.taskid = t.taskid
                            JOIN Users u ON s.studentid = u.userid
                            ORDER BY s.submittedat DESC";

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                submissions.Add(new TaskInfo
                                {
                                    SubmissionId = reader.GetInt32(0),
                                    TaskId = reader.GetInt32(1),
                                    StudentId = reader.GetInt32(2),
                                    SubmissionDate = reader.GetDateTime(3),
                                    Status = (SubmissionStatus)Enum.Parse(typeof(SubmissionStatus), reader.GetString(4)),
                                    FilePath = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Comments = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Feedback = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    TaskTitle = reader.GetString(7),
                                    SubmitterName = reader.GetString(8)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении работ: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return submissions;
        }

        /// <summary>
        /// Authenticates a user
        /// Аутентифицирует пользователя
        /// </summary>
        /// <param name="username">Username
        /// Имя пользователя</param>
        /// <param name="password">Password
        /// Пароль</param>
        /// <returns>User information if authentication successful, null otherwise
        /// Информация о пользователе в случае успешной аутентификации, null в противном случае</returns>
        public (int UserId, string FullName, string Role)? AuthenticateUser(string username, string password)
        {
            try
            {
                var hashedPassword = HashPassword(password);
                

                using (var connection = GetConnection())
                {
                    connection.Open();
                    

                    // Сначала проверим, существует ли пользователь
                    using (var checkCommand = new NpgsqlCommand("SELECT COUNT(*) FROM users WHERE username = @username", connection))
                    {
                        checkCommand.Parameters.AddWithValue("@username", username);
                        var userCount = Convert.ToInt32(checkCommand.ExecuteScalar());
                        
                    }

                    // Теперь попробуем аутентификацию
                    using (var command = new NpgsqlCommand("SELECT * FROM authenticate_user(@username, @password)", connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", hashedPassword);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var userId = reader.GetInt32(0);
                                var fullName = reader.GetString(1);
                                var role = reader.GetString(2);
                                return (userId, fullName, role);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при аутентификации пользователя: {ex.Message}\nStack trace: {ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        /// <summary>
        /// Hashes a password using SHA-256
        /// Хеширует пароль используя SHA-256
        /// </summary>
        /// <param name="password">Password to hash
        /// Пароль для хеширования</param>
        /// <returns>Hashed password as a hexadecimal string
        /// Хешированный пароль в виде шестнадцатеричной строки</returns>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Gets a database connection
        /// Получает соединение с базой данных
        /// </summary>
        /// <returns>NpgsqlConnection object
        /// Объект NpgsqlConnection</returns>
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        /// <summary>
        /// Gets submissions by student ID
        /// Получает работы по ID студента
        /// </summary>
        /// <param name="studentId">Student ID / ID студента</param>
        /// <returns>List of submissions / Список работ</returns>
        public List<TaskInfo> GetSubmissionsByStudentId(int studentId)
        {
            var submissions = new List<TaskInfo>();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            SELECT s.submissionid, s.taskid, s.studentid, s.submittedat, s.status, s.filename, s.feedback,
                                   t.title as tasktitle, u.fullname as studentname
                            FROM Submissions s
                            JOIN Tasks t ON s.taskid = t.taskid
                            JOIN Users u ON s.studentid = u.userid
                            WHERE s.studentid = @studentId
                            ORDER BY s.submittedat DESC";

                        command.Parameters.AddWithValue("@studentId", studentId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                submissions.Add(new TaskInfo
                                {
                                    SubmissionId = reader.GetInt32(0),
                                    TaskId = reader.GetInt32(1),
                                    StudentId = reader.GetInt32(2),
                                    SubmissionDate = reader.GetDateTime(3),
                                    Status = (SubmissionStatus)Enum.Parse(typeof(SubmissionStatus), reader.GetString(4)),
                                    FilePath = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Comments = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Feedback = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    TaskTitle = reader.GetString(7),
                                    SubmitterName = reader.GetString(8)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении работ: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return submissions;
        }

        /// <summary>
        /// Gets submissions by student name
        /// Получает отправленные работы по имени студента
        /// </summary>
        /// <param name="studentName">Student name (can be partial)
        /// Имя студента (может быть частичным)</param>
        /// <returns>List of TaskInfo objects containing submission information
        /// Список объектов TaskInfo, содержащих информацию об отправленных работах</returns>
        public List<TaskInfo> GetSubmissionsByStudentName(string studentName)
        {
            var submissions = new List<TaskInfo>();
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(@"
                        SELECT 
                            s.SubmissionId,
                            s.StudentId,
                            t.Title,
                            u.FullName,
                            s.FileName,
                            s.SubmittedAt,
                            s.Status,
                            s.Feedback
                        FROM Submissions s
                        JOIN Tasks t ON s.TaskId = t.TaskId
                        JOIN Users u ON s.StudentId = u.UserId
                        WHERE u.FullName ILIKE @studentName
                        ORDER BY s.SubmittedAt DESC", connection))
                    {
                        command.Parameters.AddWithValue("@studentName", $"%{studentName}%");
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                submissions.Add(new TaskInfo
                                {
                                    SubmissionId = reader.GetInt32(0),
                                    StudentId = reader.GetInt32(1),
                                    TaskTitle = reader.GetString(2),
                                    SubmitterName = reader.GetString(3),
                                    FilePath = reader.GetString(4),
                                    SubmissionDate = reader.GetDateTime(5),
                                    Status = (SubmissionStatus)Enum.Parse(typeof(SubmissionStatus), reader.GetString(6)),
                                    Comments = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    Feedback = reader.IsDBNull(7) ? null : reader.GetString(7)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении работ студента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return submissions;
        }

        /// <summary>
        /// Updates the status of a submission
        /// Обновляет статус отправленной работы
        /// </summary>
        /// <param name="submissionId">ID of the submission to update
        /// ID отправленной работы для обновления</param>
        /// <param name="status">New status (Approved/Rejected)
        /// Новый статус (Approved/Rejected)</param>
        /// <param name="feedback">Review feedback
        /// Комментарий проверки</param>
        /// <param name="reviewerId">ID of the reviewer
        /// ID проверяющего</param>
        /// <returns>True if update was successful, false otherwise
        /// True если обновление прошло успешно, false в противном случае</returns>
        public bool UpdateSubmissionStatus(int submissionId, string status, string feedback, int reviewerId)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(@"
                        UPDATE Submissions 
                        SET Status = @status,
                            Feedback = @feedback,
                            ReviewerId = @reviewerId,
                            ReviewedAt = CURRENT_TIMESTAMP
                        WHERE SubmissionId = @submissionId", connection))
                    {
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@feedback", feedback);
                        command.Parameters.AddWithValue("@reviewerId", reviewerId);
                        command.Parameters.AddWithValue("@submissionId", submissionId);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса работы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Gets submission by student ID and task ID
        /// Получает работу по ID студента и ID задания
        /// </summary>
        /// <param name="studentId">Student ID / ID студента</param>
        /// <param name="taskId">Task ID / ID задания</param>
        /// <returns>TaskInfo object or null if not found / Объект TaskInfo или null, если не найден</returns>
        public TaskInfo GetSubmissionByStudentAndTask(int studentId, int taskId)
        {
            try
            {
                Console.WriteLine($"Поиск отправки: StudentId={studentId}, TaskId={taskId}");
                
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            SELECT s.submissionid, s.taskid, s.studentid, s.submittedat, s.status, s.filename, s.feedback,
                                   t.title as tasktitle, u.fullname as studentname
                            FROM Submissions s
                            JOIN Tasks t ON s.taskid = t.taskid
                            JOIN Users u ON s.studentid = u.userid
                            WHERE s.taskid = @taskId AND s.studentid = @studentId
                            ORDER BY s.submittedat DESC";

                        command.Parameters.AddWithValue("@taskId", taskId);
                        command.Parameters.AddWithValue("@studentId", studentId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var submission = new TaskInfo
                                {
                                    SubmissionId = reader.GetInt32(0),
                                    TaskId = reader.GetInt32(1),
                                    StudentId = reader.GetInt32(2),
                                    SubmissionDate = reader.GetDateTime(3),
                                    Status = (SubmissionStatus)Enum.Parse(typeof(SubmissionStatus), reader.GetString(4)),
                                    FilePath = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Comments = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Feedback = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    TaskTitle = reader.GetString(7),
                                    SubmitterName = reader.GetString(8)
                                };
                                
                                Console.WriteLine($"Найдена отправка: SubmissionId={submission.SubmissionId}, Status={submission.Status}, Feedback={submission.Feedback}");
                                return submission;
                            }
                            else
                            {
                                Console.WriteLine("Отправка не найдена в базе данных");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении работы: {ex.Message}");
                MessageBox.Show($"Ошибка при получении работы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        /// <summary>
        /// Gets submissions by task ID
        /// Получает работы по ID задания
        /// </summary>
        /// <param name="taskId">Task ID / ID задания</param>
        /// <returns>List of submissions / Список работ</returns>
        public List<TaskInfo> GetSubmissionsByTaskId(int taskId)
        {
            var submissions = new List<TaskInfo>();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            SELECT s.submissionid, s.taskid, s.studentid, s.submittedat, s.status, s.filename, s.feedback,
                                   t.title as tasktitle, u.fullname as studentname
                            FROM Submissions s
                            JOIN Tasks t ON s.taskid = t.taskid
                            JOIN Users u ON s.studentid = u.userid
                            WHERE s.taskid = @taskId
                            ORDER BY s.submittedat DESC";

                        command.Parameters.AddWithValue("@taskId", taskId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                submissions.Add(new TaskInfo
                                {
                                    SubmissionId = reader.GetInt32(0),
                                    TaskId = reader.GetInt32(1),
                                    StudentId = reader.GetInt32(2),
                                    SubmissionDate = reader.GetDateTime(3),
                                    Status = (SubmissionStatus)Enum.Parse(typeof(SubmissionStatus), reader.GetString(4)),
                                    FilePath = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    Comments = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    Feedback = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    TaskTitle = reader.GetString(7),
                                    SubmitterName = reader.GetString(8)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении работ: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return submissions;
        }

        /// <summary>
        /// Submits a task for a student
        /// </summary>
        /// <param name="taskId">The ID of the task</param>
        /// <param name="studentId">The ID of the student</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="fileData">The binary data of the file</param>
        /// <param name="fileType">The type of the file</param>
        /// <returns>The ID of the created submission</returns>
        public int SubmitTask(int taskId, int studentId, string fileName, byte[] fileData, string fileType)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT submit_task(@taskId, @studentId, @fileName, @fileData, @fileType)", connection))
                    {
                        command.Parameters.AddWithValue("@taskId", taskId);
                        command.Parameters.AddWithValue("@studentId", studentId);
                        command.Parameters.AddWithValue("@fileName", fileName);
                        command.Parameters.AddWithValue("@fileData", fileData);
                        command.Parameters.AddWithValue("@fileType", fileType);

                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке задания: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Gets all study materials for all tasks
        /// </summary>
        /// <returns>List of study materials with task information</returns>
        public List<StudyMaterialInfo> GetAllStudyMaterials()
        {
            var materials = new List<StudyMaterialInfo>();
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(@"
                        SELECT 
                            m.MaterialId,
                            m.TaskId,
                            t.Title as TaskTitle,
                            m.Title,
                            m.Description,
                            m.FileName,
                            m.FileType,
                            u.FullName as UploadedByName,
                            m.UploadedAt
                        FROM StudyMaterials m
                        JOIN Tasks t ON m.TaskId = t.TaskId
                        JOIN Users u ON m.UploadedBy = u.UserId
                        ORDER BY t.Title, m.UploadedAt DESC", connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                materials.Add(new StudyMaterialInfo
                                {
                                    MaterialId = reader.GetInt32(0),
                                    TaskId = reader.GetInt32(1),
                                    TaskTitle = reader.GetString(2),
                                    Title = reader.GetString(3),
                                    Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    FileName = reader.GetString(5),
                                    FileType = reader.GetString(6),
                                    UploadedByName = reader.GetString(7),
                                    UploadedAt = reader.GetDateTime(8)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении методических материалов: {ex.Message}");
            }
            return materials;
        }

        /// <summary>
        /// Downloads a study material to a specified directory
        /// </summary>
        /// <param name="materialId">The ID of the study material</param>
        /// <param name="targetDirectory">The directory to save the file to</param>
        /// <returns>The path to the downloaded file, or null if download failed</returns>
        public string DownloadStudyMaterial(int materialId, string targetDirectory)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(@"
                        SELECT FileName, FileData, FileType
                        FROM StudyMaterials
                        WHERE MaterialId = @materialId", connection))
                    {
                        command.Parameters.AddWithValue("@materialId", materialId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fileName = reader.GetString(0);
                                byte[] fileData = (byte[])reader.GetValue(1);
                                string fileType = reader.GetString(2);

                                string filePath = Path.Combine(targetDirectory, fileName);
                                File.WriteAllBytes(filePath, fileData);
                                return filePath;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при скачивании методического материала: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Adds a new study material to the database
        /// </summary>
        /// <param name="title">Title of the material</param>
        /// <param name="description">Description of the material</param>
        /// <param name="filePath">Path to the material file</param>
        /// <param name="uploadedBy">ID of the user who uploaded the material</param>
        /// <param name="taskId">ID of the task this material is associated with</param>
        /// <returns>True if the material was added successfully, false otherwise</returns>
        public bool AddStudyMaterial(string title, string description, string filePath, int uploadedBy, int taskId)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Read file data
                    byte[] fileData = File.ReadAllBytes(filePath);
                    string fileName = Path.GetFileName(filePath);
                    string fileType = Path.GetExtension(filePath).TrimStart('.');

                    string query = @"
                        insert into studymaterials 
                        (title, description, filename, filedata, filetype, uploadedby, taskid) 
                        values 
                        (@title, @description, @filename, @filedata, @filetype, @uploadedby, @taskid) 
                        returning materialid";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@filename", fileName);
                        command.Parameters.AddWithValue("@filedata", fileData);
                        command.Parameters.AddWithValue("@filetype", fileType);
                        command.Parameters.AddWithValue("@uploadedby", uploadedBy);
                        command.Parameters.AddWithValue("@taskid", taskId);

                        var result = command.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении материала: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Adds a new user to the database
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password (will be hashed before storage)</param>
        /// <param name="fullName">User's full name</param>
        /// <param name="role">User role (Teacher or Student)</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool AddUser(string username, string password, string fullName, string role)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            insert into users (username, password, fullname, role)
                            values (@username, @password, @fullname, @role)
                            returning userid";

                        string hashedPassword = HashPassword(password);

                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", hashedPassword);
                        command.Parameters.AddWithValue("@fullname", fullName);
                        command.Parameters.AddWithValue("@role", role);

                        var result = command.ExecuteScalar();
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Adds a new task to the database
        /// </summary>
        /// <param name="taskName">Name of the task</param>
        /// <param name="description">Task description</param>
        /// <param name="userid">ID of the user creating the task</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool AddTask(string taskName, string description, int userid)
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            INSERT INTO tasks (title, description, creatorid)
                            VALUES (@title, @description, @creatorid)";

                        command.Parameters.AddWithValue("@title", taskName);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@creatorid", userid);
                        
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении задания: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Gets all tasks from the database
        /// </summary>
        /// <returns>List of Task objects</returns>
        public List<TaskAdd> GetAllTasks()
        {
            List<TaskAdd> tasks = new List<TaskAdd>();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT t.taskid, t.title, t.description, t.creatorid, t.createdat, t.isactive 
                        FROM tasks t
                        JOIN users u ON t.creatorid = u.userid
                        WHERE t.isactive = true";

                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(new TaskAdd
                                {
                                    TaskId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    CreatorId = reader.GetInt32(3),
                                    CreatedAt = reader.GetDateTime(4),
                                    IsActive = reader.GetBoolean(5)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении списка заданий: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return tasks;
        }

        /// <summary>
        /// Deletes a user and all related data from the database by username
        /// </summary>
        /// <param name="username">Username of the user to delete</param>
        /// <returns>True if the user was deleted successfully, false otherwise</returns>
        public bool DeleteUser(string username)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // First, check if user exists and get their ID
                    int userId = -1;
                    using (NpgsqlCommand checkCommand = new NpgsqlCommand())
                    {
                        checkCommand.Connection = connection;
                        checkCommand.CommandText = "SELECT UserId FROM users WHERE username = @username";
                        checkCommand.Parameters.AddWithValue("@username", username);
                        
                        var result = checkCommand.ExecuteScalar();
                        if (result == null)
                        {
                            return false; // User not found
                        }
                        userId = Convert.ToInt32(result);
                    }
                    
                    // Begin transaction for safe deletion
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 1. Delete submissions where user is reviewer (ReviewerId)
                            using (NpgsqlCommand deleteReviewerSubmissionsCommand = new NpgsqlCommand())
                            {
                                deleteReviewerSubmissionsCommand.Connection = connection;
                                deleteReviewerSubmissionsCommand.Transaction = transaction;
                                deleteReviewerSubmissionsCommand.CommandText = "DELETE FROM submissions WHERE reviewerid = @userId";
                                deleteReviewerSubmissionsCommand.Parameters.AddWithValue("@userId", userId);
                                deleteReviewerSubmissionsCommand.ExecuteNonQuery();
                            }
                            
                            // 2. Delete submissions where user is student (StudentId)
                            using (NpgsqlCommand deleteStudentSubmissionsCommand = new NpgsqlCommand())
                            {
                                deleteStudentSubmissionsCommand.Connection = connection;
                                deleteStudentSubmissionsCommand.Transaction = transaction;
                                deleteStudentSubmissionsCommand.CommandText = "DELETE FROM submissions WHERE studentid = @userId";
                                deleteStudentSubmissionsCommand.Parameters.AddWithValue("@userId", userId);
                                deleteStudentSubmissionsCommand.ExecuteNonQuery();
                            }
                            
                            // 3. Delete study materials uploaded by user
                            using (NpgsqlCommand deleteMaterialsCommand = new NpgsqlCommand())
                            {
                                deleteMaterialsCommand.Connection = connection;
                                deleteMaterialsCommand.Transaction = transaction;
                                deleteMaterialsCommand.CommandText = "DELETE FROM studymaterials WHERE uploadedby = @userId";
                                deleteMaterialsCommand.Parameters.AddWithValue("@userId", userId);
                                deleteMaterialsCommand.ExecuteNonQuery();
                            }
                            
                            // 4. Delete tasks created by user
                            using (NpgsqlCommand deleteTasksCommand = new NpgsqlCommand())
                            {
                                deleteTasksCommand.Connection = connection;
                                deleteTasksCommand.Transaction = transaction;
                                deleteTasksCommand.CommandText = "DELETE FROM tasks WHERE creatorid = @userId";
                                deleteTasksCommand.Parameters.AddWithValue("@userId", userId);
                                deleteTasksCommand.ExecuteNonQuery();
                            }
                            
                            // 5. Finally, delete the user
                            using (NpgsqlCommand deleteUserCommand = new NpgsqlCommand())
                            {
                                deleteUserCommand.Connection = connection;
                                deleteUserCommand.Transaction = transaction;
                                deleteUserCommand.CommandText = "DELETE FROM users WHERE userid = @userId";
                                deleteUserCommand.Parameters.AddWithValue("@userId", userId);
                                deleteUserCommand.ExecuteNonQuery();
                            }
                            
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Tests database connection and returns basic information
        /// </summary>
        /// <returns>True if connection successful, false otherwise</returns>
        public bool TestConnection()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Подключение к базе данных успешно");
                    
                    // Test query to check if tables exist
                    using (var command = new NpgsqlCommand("SELECT COUNT(*) FROM submissions", connection))
                    {
                        var count = Convert.ToInt32(command.ExecuteScalar());
                        Console.WriteLine($"Количество записей в таблице submissions: {count}");
                    }
                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения к базе данных: {ex.Message}");
                return false;
            }
        }
    }
}