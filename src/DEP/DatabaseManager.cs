using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Npgsql;

namespace DEP.Database
{
    /// <summary>
    /// Manages database operations for the exam preparation system
    /// </summary>
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        private readonly string _connectionString;

        /// <summary>
        /// Gets the singleton instance of DatabaseManager
        /// </summary>
        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseManager();
                }
                return _instance;
            }
        }

        private DatabaseManager()
        {
            _connectionString = "Host=116.202.229.219;Database=exampapers;Username=dodep;Password=dodep";
        }

        /// <summary>
        /// Authenticates a user with the given username and password
        /// </summary>
        /// <param name="username">The username to authenticate</param>
        /// <param name="password">The password to authenticate</param>
        /// <returns>A tuple containing user ID, full name, and role if authentication is successful</returns>
        public (int UserId, string FullName, string Role)? AuthenticateUser(string username, string password)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM authenticate_user(@username, @password)", connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", HashPassword(password));

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2)
                                );
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при аутентификации пользователя: {ex.Message}");
                return null;
            }
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
                using (var connection = new NpgsqlConnection(_connectionString))
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
        /// Gets all submissions for a student
        /// </summary>
        /// <param name="studentId">The ID of the student</param>
        /// <returns>A DataTable containing the student's submissions</returns>
        public DataTable GetStudentSubmissions(int studentId)
        {
            var result = new DataTable();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM get_student_submissions(@studentId)", connection))
                    {
                        command.Parameters.AddWithValue("@studentId", studentId);
                        using (var adapter = new NpgsqlDataAdapter(command))
                        {
                            adapter.Fill(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении заданий студента: {ex.Message}");
            }
            return result;
        }

        /// <summary>
        /// Gets all submissions for review
        /// </summary>
        /// <returns>A DataTable containing all submissions</returns>
        public DataTable GetSubmissionsForReview()
        {
            var result = new DataTable();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT * FROM get_submissions_for_review()", connection))
                    {
                        using (var adapter = new NpgsqlDataAdapter(command))
                        {
                            adapter.Fill(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении заданий для проверки: {ex.Message}");
            }
            return result;
        }

        /// <summary>
        /// Updates the status of a submission
        /// </summary>
        /// <param name="submissionId">The ID of the submission</param>
        /// <param name="status">The new status</param>
        /// <param name="feedback">The feedback for the submission</param>
        /// <param name="reviewerId">The ID of the reviewer</param>
        /// <returns>True if the update was successful</returns>
        public bool UpdateSubmissionStatus(int submissionId, string status, string feedback, int reviewerId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT update_submission_status(@submissionId, @status, @feedback, @reviewerId)", connection))
                    {
                        command.Parameters.AddWithValue("@submissionId", submissionId);
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.AddWithValue("@feedback", feedback);
                        command.Parameters.AddWithValue("@reviewerId", reviewerId);

                        return Convert.ToBoolean(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении статуса задания: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Downloads a submission file
        /// </summary>
        /// <param name="submissionId">The ID of the submission</param>
        /// <param name="savePath">The path where to save the file</param>
        /// <returns>True if the download was successful</returns>
        public bool DownloadSubmission(int submissionId, string savePath)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand("SELECT FileName, FileData FROM Submissions WHERE SubmissionId = @submissionId", connection))
                    {
                        command.Parameters.AddWithValue("@submissionId", submissionId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var fileName = reader.GetString(0);
                                var fileData = (byte[])reader.GetValue(1);
                                System.IO.File.WriteAllBytes(System.IO.Path.Combine(savePath, fileName), fileData);
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при скачивании файла: {ex.Message}");
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
} 