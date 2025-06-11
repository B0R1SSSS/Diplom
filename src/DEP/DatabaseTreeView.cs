using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace DEP
{
    /// <summary>
    /// Custom TreeView control for displaying database tasks
    /// </summary>
    public class DatabaseTreeView : TreeView
    {
        private int currentUserId;
        private string currentUserRole;

        /// <summary>
        /// Event that occurs when a task is selected
        /// </summary>
        public event EventHandler<TaskSelectedEventArgs> TaskSelected;

        /// <summary>
        /// Initializes a new instance of the DatabaseTreeView class
        /// </summary>
        public DatabaseTreeView()
        {
            this.AfterSelect += DatabaseTreeView_AfterSelect;
        }

        /// <summary>
        /// Loads tasks from database based on user role
        /// </summary>
        /// <param name="userId">Current user ID</param>
        /// <param name="userRole">Current user role</param>
        public void LoadTasks(int userId, string userRole)
        {
            try
            {
                currentUserId = userId;
                currentUserRole = userRole;

                this.Nodes.Clear();

                // Create root nodes for different task statuses
                var pendingNode = new TreeNode("Ожидающие проверки");
                var approvedNode = new TreeNode("Одобренные");
                var rejectedNode = new TreeNode("Отклоненные");

                // Get tasks based on user role
                List<TaskInfo> tasks;
                if (userRole == "admin" || userRole == "teacher")
                {
                    tasks = DatabaseManager.Instance.GetSubmissionsForReview();
                }
                else
                {
                    tasks = DatabaseManager.Instance.GetStudentSubmissions(userId);
                }

                // Group tasks by status
                foreach (var task in tasks)
                {
                    var taskNode = new TreeNode($"{task.TaskTitle} - {task.SubmitterName}")
                    {
                        Tag = task
                    };

                    switch (task.Status)
                    {
                        case "pending":
                            pendingNode.Nodes.Add(taskNode);
                            break;
                        case "approved":
                            approvedNode.Nodes.Add(taskNode);
                            break;
                        case "rejected":
                            rejectedNode.Nodes.Add(taskNode);
                            break;
                    }
                }

                // Add nodes to tree
                if (pendingNode.Nodes.Count > 0) this.Nodes.Add(pendingNode);
                if (approvedNode.Nodes.Count > 0) this.Nodes.Add(approvedNode);
                if (rejectedNode.Nodes.Count > 0) this.Nodes.Add(rejectedNode);

                // Expand all nodes
                this.ExpandAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заданий: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Refreshes the tree view with current data
        /// </summary>
        public void RefreshTreeView()
        {
            LoadTasks(currentUserId, currentUserRole);
        }

        private void DatabaseTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is TaskInfo taskInfo)
            {
                TaskSelected?.Invoke(this, new TaskSelectedEventArgs(taskInfo));
            }
        }
    }

    /// <summary>
    /// Class containing task information
    /// </summary>
    public class TaskInfo
    {
        public int TaskId { get; set; }
        public string TaskTitle { get; set; }
        public int SubmitterId { get; set; }
        public string SubmitterName { get; set; }
        public string FilePath { get; set; }
        public string Status { get; set; }
        public DateTime SubmissionDate { get; set; }
    }

    /// <summary>
    /// Event arguments for task selection
    /// </summary>
    public class TaskSelectedEventArgs : EventArgs
    {
        public TaskInfo TaskInfo { get; }

        public TaskSelectedEventArgs(TaskInfo taskInfo)
        {
            TaskInfo = taskInfo;
        }
    }
}