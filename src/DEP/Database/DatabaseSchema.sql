-- Create Users table
CREATE TABLE Users (
    UserId SERIAL PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(64) NOT NULL, -- SHA-256 hash
    FullName VARCHAR(100) NOT NULL,
    Role VARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Teacher', 'Student')),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    LastLogin TIMESTAMP
);

-- Create Tasks table
CREATE TABLE Tasks (
    TaskId SERIAL PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Description TEXT,
    CreatorId INTEGER REFERENCES Users(UserId),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IsActive BOOLEAN DEFAULT TRUE
);

-- Create Submissions table
CREATE TABLE Submissions (
    SubmissionId SERIAL PRIMARY KEY,
    TaskId INTEGER REFERENCES Tasks(TaskId),
    StudentId INTEGER REFERENCES Users(UserId),
    FileName VARCHAR(255) NOT NULL,
    FileData BYTEA NOT NULL,
    FileType VARCHAR(50) NOT NULL,
    SubmittedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(20) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Approved', 'Rejected')),
    Feedback TEXT,
    ReviewerId INTEGER REFERENCES Users(UserId),
    ReviewedAt TIMESTAMP
);

-- Create indexes
CREATE INDEX idx_users_username ON Users(Username);
CREATE INDEX idx_submissions_task ON Submissions(TaskId);
CREATE INDEX idx_submissions_student ON Submissions(StudentId);
CREATE INDEX idx_submissions_status ON Submissions(Status);

-- Insert test data for Users
INSERT INTO Users (Username, Password, FullName, Role) VALUES
    ('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Admin User', 'Admin'),
    ('teacher1', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'John Smith', 'Teacher'),
    ('teacher2', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Mary Johnson', 'Teacher'),
    ('student1', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Alice Brown', 'Student'),
    ('student2', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Bob Wilson', 'Student');

-- Insert test data for Tasks
INSERT INTO Tasks (Title, Description, CreatorId, IsActive) VALUES
    ('Database Design', 'Design a normalized database schema for an e-commerce system', 2, true),
    ('Web Development', 'Create a responsive website using HTML, CSS, and JavaScript', 2, true),
    ('Data Analysis', 'Analyze sales data and create visualizations', 3, true),
    ('System Architecture', 'Design a microservices architecture for a banking system', 3, true),
    ('Security Assessment', 'Perform a security audit of a web application', 2, true);

-- Insert test data for Submissions
INSERT INTO Submissions (TaskId, StudentId, FileName, FileData, FileType, Status, Feedback) VALUES
    (1, 4, 'database_design.pdf', E'\\x504B0304', 'PDF', 'Approved', 'Excellent work! Very well structured.'),
    (2, 4, 'website.zip', E'\\x504B0304', 'ZIP', 'Pending', NULL),
    (3, 5, 'analysis.xlsx', E'\\x504B0304', 'XLSX', 'Rejected', 'Please add more detailed explanations.'),
    (4, 5, 'architecture.docx', E'\\x504B0304', 'DOCX', 'Pending', NULL),
    (1, 5, 'db_design_v2.pdf', E'\\x504B0304', 'PDF', 'Approved', 'Good improvements from the previous version.');

-- Create stored procedures
CREATE OR REPLACE FUNCTION authenticate_user(
    p_username VARCHAR,
    p_password VARCHAR
) RETURNS TABLE (
    user_id INTEGER,
    full_name VARCHAR,
    role VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT u.UserId, u.FullName, u.Role
    FROM Users u
    WHERE u.Username = p_username
    AND u.Password = p_password;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION submit_task(
    p_task_id INTEGER,
    p_student_id INTEGER,
    p_file_name VARCHAR,
    p_file_data BYTEA,
    p_file_type VARCHAR
) RETURNS INTEGER AS $$
DECLARE
    v_submission_id INTEGER;
BEGIN
    INSERT INTO Submissions (TaskId, StudentId, FileName, FileData, FileType)
    VALUES (p_task_id, p_student_id, p_file_name, p_file_data, p_file_type)
    RETURNING SubmissionId INTO v_submission_id;
    
    RETURN v_submission_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_student_submissions(
    p_student_id INTEGER
) RETURNS TABLE (
    submission_id INTEGER,
    task_title VARCHAR,
    file_name VARCHAR,
    submitted_at TIMESTAMP,
    status VARCHAR,
    feedback TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT s.SubmissionId, t.Title, s.FileName, s.SubmittedAt, s.Status, s.Feedback
    FROM Submissions s
    JOIN Tasks t ON s.TaskId = t.TaskId
    WHERE s.StudentId = p_student_id
    ORDER BY s.SubmittedAt DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_submissions_for_review() RETURNS TABLE (
    submission_id INTEGER,
    task_title VARCHAR,
    student_name VARCHAR,
    file_name VARCHAR,
    submitted_at TIMESTAMP,
    status VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT s.SubmissionId, t.Title, u.FullName, s.FileName, s.SubmittedAt, s.Status
    FROM Submissions s
    JOIN Tasks t ON s.TaskId = t.TaskId
    JOIN Users u ON s.StudentId = u.UserId
    ORDER BY s.SubmittedAt DESC;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION update_submission_status(
    p_submission_id INTEGER,
    p_status VARCHAR,
    p_feedback TEXT,
    p_reviewer_id INTEGER
) RETURNS BOOLEAN AS $$
BEGIN
    UPDATE Submissions
    SET Status = p_status,
        Feedback = p_feedback,
        ReviewerId = p_reviewer_id,
        ReviewedAt = CURRENT_TIMESTAMP
    WHERE SubmissionId = p_submission_id;
    
    RETURN FOUND;
END;
$$ LANGUAGE plpgsql; 