using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ScholarshipManagementSystem
{
    class Program
    {
        static void Main()
        {
            try
            {
                MainMenu mainMenu = new MainMenu();
                mainMenu.Display();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    abstract class Menu
    {
        protected void DisplayHeader(string title)
        {
            Console.Clear();
            Console.WriteLine($"     +++{title}+++");
            
        }
    }

    class MainMenu : Menu
    {
        public void Display()
        {
            while (true)
            {
                DisplayHeader("Cebu Institute of Technology");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        Login login = new Login();
                        login.Authenticate();
                        break;
                    case "2":
                        RegisterApplicant();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void RegisterApplicant()
        {
            Console.Clear();
            Console.WriteLine("========== Applicant Registration ==========");
            string username, password, name;

            do
            {
                Console.Write("Enter your desired username: ");
                username = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(username))
                {
                    Console.WriteLine("Username cannot be empty. Please try again.");
                }
            } while (string.IsNullOrWhiteSpace(username));

            do
            {
                Console.Write("Enter your password: ");
                password = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(password))
                {
                    Console.WriteLine("Password cannot be empty. Please try again.");
                }
            } while (string.IsNullOrWhiteSpace(password));

            do
            {
                Console.Write("Enter your name: ");
                name = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Name cannot be empty. Please try again.");
                }
            } while (string.IsNullOrWhiteSpace(name));

            string data = $"{username}|{password}|{name}";
            try
            {
                File.AppendAllText("students.txt", data + Environment.NewLine);
                Console.WriteLine("Registration successful! You can now log in.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error during registration: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
        }
    }

    class Login : Menu
    {
        public void Authenticate()
        {
            DisplayHeader("Cebu Institute of Technology - Login");
            Console.Write("1. Username: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("2. Password: ");
            string password = Console.ReadLine() ?? "";

            try
            {
                if (username == "" && password == "ADMIN")
                {
                    AdminMenu adminMenu = new AdminMenu();
                    adminMenu.Display();
                }
                else if (File.Exists("students.txt") &&
                         File.ReadLines("students.txt").Any(line => line.StartsWith($"{username}|{password}|")))
                {
                    ApplicantMenu applicantMenu = new ApplicantMenu(username);
                    applicantMenu.Display();
                }
                else
                {
                    Console.WriteLine("Invalid credentials. Press any key to try again.");
                    Console.ReadKey();
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error during authentication: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }

    class AdminMenu : Menu
    {
        public void Display()
        {
            while (true)
            {
                DisplayHeader("Admin Menu - Cebu Institute of Technology");
                Console.WriteLine("1. Create Scholarship");
                Console.WriteLine("2. View Scholarships");
                Console.WriteLine("3. Update Scholarship");
                Console.WriteLine("4. Delete Scholarship");
                Console.WriteLine("5. View Applicants");
                Console.WriteLine("6. View Registered Applicants");
                Console.WriteLine("7. Search Scholarships");
                Console.WriteLine("8. Manage Accounts");
                Console.WriteLine("9. Logout");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        ScholarshipManager.CreateScholarship();
                        break;
                    case "2":
                        ScholarshipManager.ViewScholarships();
                        break;
                    case "3":
                        ScholarshipManager.UpdateScholarship();
                        break;
                    case "4":
                        ScholarshipManager.DeleteScholarship();
                        break;
                    case "5":
                        ApplicantManager.ViewApplicants();
                        break;
                    case "6":
                        ApplicantManager.ViewRegisteredApplicants();
                        break;
                    case "7":
                        ScholarshipManager.Search();
                        break;
                    case "8":
                        ManageAccounts();
                        break;
                    case "9":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ManageAccounts()
        {
            while (true)
            {
                DisplayHeader("Manage Accounts");
                if (!File.Exists("students.txt"))
                {
                    Console.WriteLine("No registered accounts found.");
                    Console.WriteLine("Press any key to return to the menu.");
                    Console.ReadKey();
                    return;
                }

                try
                {
                    var accounts = File.ReadAllLines("students.txt").ToList();
                    Console.WriteLine("Registered Accounts:");
                    for (int i = 0; i < accounts.Count; i++)
                    {
                        var parts = accounts[i].Split('|');
                        Console.WriteLine($"{i + 1}. Username: {parts[0]}, Name: {parts[2]}");
                    }
                    Console.WriteLine();
                    Console.WriteLine("1. Update Account");
                    Console.WriteLine("2. Delete Account");
                    Console.WriteLine("3. Return to Main Menu");
                    Console.Write("Choose an option: ");
                    string choice = Console.ReadLine() ?? "";
                    switch (choice)
                    {
                        case "1":
                            UpdateAccount(accounts);
                            break;
                        case "2":
                            DeleteAccount(accounts);
                            break;
                        case "3":
                            return;
                        default:
                            Console.WriteLine("Invalid option. Press any key to try again.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine($"Error managing accounts: {ioEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
        }

        private void UpdateAccount(List<string> accounts)
        {
            Console.Write("Enter the account number to update: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > accounts.Count)
            {
                Console.WriteLine("Invalid account number. Press any key to continue.");
                Console.ReadKey();
                return;
            }
            index--; // Adjust for zero-based index
            var parts = accounts[index].Split('|');
            Console.WriteLine($"Current Username: {parts[0]}, Name: {parts[2]}");

            // Validate new username
            Console.Write("Enter new username (leave blank to keep unchanged): ");
            string newUsername = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                newUsername = parts[0]; // Keep current username if input is empty
            }
            // Validate new password
            Console.Write("Enter new password (leave blank to keep unchanged): ");
            string newPassword = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                newPassword = parts[1]; // Keep current password if input is empty
            }
            // Validate new name
            Console.Write("Enter new name (leave blank to keep unchanged): ");
            string newName = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(newName))
            {
                newName = parts[2]; // Keep current name if input is empty
            }
            accounts[index] = $"{newUsername}|{newPassword}|{newName}";
            try
            {
                File.WriteAllLines("students.txt", accounts);
                Console.WriteLine("Account updated successfully. Press any key to continue.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error updating account: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.ReadKey();
        }

        private void DeleteAccount(List<string> accounts)
        {
            Console.Write("Enter the account number to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > accounts.Count)
            {
                Console.WriteLine("Invalid account number. Press any key to continue.");
                Console.ReadKey();
                return;
            }
            index--; // Adjust for zero-based index
            accounts.RemoveAt(index);
            try
            {
                File.WriteAllLines("students.txt", accounts);
                Console.WriteLine("Account deleted successfully. Press any key to continue.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error deleting account: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.ReadKey();
        }
    }

    class ApplicantMenu : Menu
    {
        private readonly string username;

        public ApplicantMenu(string username)
        {
            this.username = username;
        }

        public void Display()
        {
            while (true)
            {
                DisplayHeader("Cebu Institute of Technology");
                Console.WriteLine("1. View Available Scholarships");
                Console.WriteLine("2. Apply for Scholarship");
                Console.WriteLine("3. View Status");
                Console.WriteLine("4. Logout");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine() ?? "";
                switch (choice)
                {
                    case "1":
                        ScholarshipManager.ViewScholarships();
                        break;
                    case "2":
                        ApplicantManager.ApplyForScholarship(username);
                        break;
                    case "3":
                        ApplicantManager.ViewStatus(username);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Press any key to try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }

    static class ScholarshipManager
    {
        private const string FilePath = "scholarships.txt";

        public static void CreateScholarship()
        {
            try
            {
                Console.Write("Enter Scholarship Name: ");
                string name;
                while (string.IsNullOrWhiteSpace(name = Console.ReadLine()))
                {
                    Console.WriteLine("Scholarship Name cannot be empty. Please try again.");
                }

                Console.Write("Enter Achievement: ");
                string achievement;
                while (string.IsNullOrWhiteSpace(achievement = Console.ReadLine()))
                {
                    Console.WriteLine("Achievement cannot be empty. Please try again.");
                }

                Console.Write("Enter Benefits: ");
                string benefits;
                while (string.IsNullOrWhiteSpace(benefits = Console.ReadLine()))
                {
                    Console.WriteLine("Benefits cannot be empty. Please try again.");
                }

                Console.Write("Enter Deadline (MM/DD/YYYY): ");
                string deadline;
                while (string.IsNullOrWhiteSpace(deadline = Console.ReadLine()))
                {
                    Console.WriteLine("Deadline cannot be empty. Please try again.");
                }

                string id = new Random().Next(1000, 9999).ToString();
                string data = $"{id}|{name}|{achievement}|{benefits}|{deadline}";
                File.AppendAllText(FilePath, data + Environment.NewLine);
                Console.WriteLine("Scholarship created successfully. Press any key to continue.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error creating scholarship: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.ReadKey();
        }

        public static void ViewScholarships()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    Console.WriteLine("No scholarships available. Press any key to continue.");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine(File.ReadAllText(FilePath));
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error reading scholarships: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        public static string GetScholarshipDetails(string id)
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return "No scholarships available.";
                }

                var scholarship = File.ReadLines(FilePath)
                    .FirstOrDefault(line => line.StartsWith(id + "|"));

                return scholarship ?? "Scholarship not found.";
            }
            catch (IOException ioEx)
            {
                return $"Error retrieving scholarship details: {ioEx.Message}";
            }
            catch (Exception ex)
            {
                return $"An unexpected error occurred: {ex.Message}";
            }
        }

        public static void UpdateScholarship()
        {
            try
            {
                Console.Write("Enter Scholarship ID to update: ");
                string id = Console.ReadLine() ?? "";
                var scholarships = File.Exists(FilePath)
                    ? File.ReadAllLines(FilePath).ToList()
                    : new List<string>();
                var scholarship = scholarships.FirstOrDefault(s => s.StartsWith(id + "|"));
                if (scholarship == null)
                {
                    Console.WriteLine("Scholarship not found. Press any key to continue.");
                    Console.ReadKey();
                    return;
                }
                Console.Write("Enter new Scholarship Name: ");
                string name = Console.ReadLine() ?? "";
                Console.Write("Enter new Achievement: ");
                string achievement = Console.ReadLine() ?? "";
                Console.Write("Enter new Benefits: ");
                string benefits = Console.ReadLine() ?? "";
                Console.Write("Enter new Deadline (MM/DD/YYYY): ");
                string deadline = Console.ReadLine() ?? "";
                string newData = $"{id}|{name}|{achievement}|{benefits}|{deadline}";
                scholarships[scholarships.IndexOf(scholarship)] = newData;
                File.WriteAllLines(FilePath, scholarships);
                Console.WriteLine("Scholarship updated successfully. Press any key to continue.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error updating scholarship: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.ReadKey();
        }

        public static void DeleteScholarship()
        {
            try
            {
                Console.Write("Enter Scholarship ID to delete: ");
                string id = Console.ReadLine() ?? "";
                var scholarships = File.Exists(FilePath)
                    ? File.ReadAllLines(FilePath).ToList()
                    : new List<string>();
                var scholarship = scholarships.FirstOrDefault(s => s.StartsWith(id + "|"));
                if (scholarship == null)
                {
                    Console.WriteLine("Scholarship not found. Press any key to continue.");
                    Console.ReadKey();
                    return;
                }
                scholarships.Remove(scholarship);
                File.WriteAllLines(FilePath, scholarships);
                Console.WriteLine("Scholarship deleted successfully. Press any key to continue.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error deleting scholarship: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.ReadKey();
        }

        public static void Search()
        {
            try
            {
                Console.Write("Enter keyword to search: ");
                string keyword = Console.ReadLine() ?? "";
                var scholarships = File.Exists(FilePath)
                    ? File.ReadLines(FilePath).Where(line => line.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList()
                    : new List<string>();

                if (!scholarships.Any())
                {
                    Console.WriteLine("No scholarships found matching your keyword.");
                }
                else
                {
                    Console.WriteLine("Matching Scholarships:");
                    scholarships.ForEach(Console.WriteLine);
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error searching scholarships: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }

    static class ApplicantManager
    {
        private const string ApplicantsFilePath = "applicants.txt";
        private const string RegisteredApplicantsFilePath = "registered_applicants.txt";

        public static void ApplyForScholarship(string username)
        {
            try
            {
                ScholarshipManager.ViewScholarships();
                string scholarshipId;
                do
                {
                    Console.Write("Enter Scholarship ID to apply for: ");
                    scholarshipId = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(scholarshipId))
                    {
                        Console.WriteLine("Scholarship ID cannot be empty. Please try again.");
                    }
                } while (string.IsNullOrWhiteSpace(scholarshipId));

                string achievement;
                do
                {
                    Console.Write("Enter your Achievement: ");
                    achievement = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(achievement))
                    {
                        Console.WriteLine("Achievement cannot be empty. Please try again.");
                    }
                } while (string.IsNullOrWhiteSpace(achievement));

                string data = $"{username}|{achievement}|{scholarshipId}|Pending";
                File.AppendAllText(ApplicantsFilePath, data + Environment.NewLine);

                Console.WriteLine("Application submitted successfully. Press any key to continue.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error applying for scholarship: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public static void ViewStatus(string username)
        {
            try
            {
                if (!File.Exists(ApplicantsFilePath))
                {
                    Console.WriteLine("No applications found. Press any key to continue.");
                    Console.ReadKey();
                    return;
                }

                var applications = File.ReadLines(ApplicantsFilePath)
                    .Where(line => line.StartsWith(username + "|"))
                    .Select(line =>
                    {
                        var parts = line.Split('|');
                        return new
                        {
                            ScholarshipId = parts[2],
                            Status = parts[3]
                        };
                    }).ToList();

                if (!applications.Any())
                {
                    Console.WriteLine("No applications found. Press any key to continue.");
                    Console.ReadKey();
                    return;
                }

                foreach (var application in applications)
                {
                    string scholarshipDetails = ScholarshipManager.GetScholarshipDetails(application.ScholarshipId);
                    Console.WriteLine($"Scholarship: {scholarshipDetails}");
                    Console.WriteLine($"Status: {application.Status}");
                    Console.WriteLine();
                }

                // Allow user to remove rejected applications
                Console.WriteLine("Do you want to remove any rejected applications? (Y/N)");
                string removeChoice = Console.ReadLine()?.ToUpper();
                if (removeChoice == "Y")
                {
                    RemoveRejectedApplication(username);
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error viewing status: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        public static void ViewApplicants()
        {
            try
            {
                if (!File.Exists(ApplicantsFilePath))
                {
                    Console.WriteLine("No applicants found. Press any key to continue.");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Scholarship Applications:");
                var applications = File.ReadLines(ApplicantsFilePath).ToList();

                foreach (var line in applications)
                {
                    var parts = line.Split('|');
                    Console.WriteLine($"Applicant: {parts[0]}, Achievement: {parts[1]}, Scholarship ID: {parts[2]}, Status: {parts[3]}");
                }
                Console.WriteLine();
                Console.WriteLine("1. Accept Application");
                Console.WriteLine("2. Reject Application");
                Console.WriteLine("3. Return to Main Menu");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine() ?? "";
                if (choice == "1" || choice == "2")
                {
                    Console.Write("Enter Applicant's Username: ");
                    string applicantUsername = Console.ReadLine() ?? "";
                    Console.Write("Enter Scholarship ID: ");
                    string scholarshipId = Console.ReadLine() ?? "";
                    string status = choice == "1" ? "Accepted" : "Rejected";
                    UpdateApplicationStatus(applicantUsername, scholarshipId, status);
                    if (status == "Accepted")
                    {
                        RegisterAcceptedApplicant(applicantUsername, scholarshipId);
                    }
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error viewing applicants: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        private static void UpdateApplicationStatus(string username, string scholarshipId, string status)
        {
            try
            {
                var applications = File.ReadAllLines(ApplicantsFilePath).ToList();

                for (int i = 0; i < applications.Count; i++)
                {
                    var parts = applications[i].Split('|');
                    if (parts[0] == username && parts[2] == scholarshipId)
                    {
                        parts[3] = status;
                        applications[i] = string.Join('|', parts);
                        break;
                    }
                }
                File.WriteAllLines(ApplicantsFilePath, applications);
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error updating application status: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private static void RegisterAcceptedApplicant(string username, string scholarshipId)
        {
            try
            {
                var scholarshipDetails = ScholarshipManager.GetScholarshipDetails(scholarshipId);
                string data = $"{username}|{scholarshipDetails}";
                File.AppendAllText(RegisteredApplicantsFilePath, data + Environment.NewLine);
                Console.WriteLine("Applicant has been registered successfully.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error registering accepted applicant: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public static void ViewRegisteredApplicants()
        {
            try
            {
                if (!File.Exists(RegisteredApplicantsFilePath))
                {
                    Console.WriteLine("No registered applicants found. Press any key to continue.");
                    Console.ReadKey();
                    return;
                }
                var applicants = File.ReadAllLines(RegisteredApplicantsFilePath).ToList();
                foreach (var applicant in applicants)
                {
                    Console.WriteLine(applicant);
                }
                Console.WriteLine();
                Console.WriteLine("1. Update Applicant");
                Console.WriteLine("2. Delete Applicant");
                Console.WriteLine("3. Return to Main Menu");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine() ?? "";
                if (choice == "1")
                {
                    Console.Write("Enter Username to update: ");
                    string username = Console.ReadLine() ?? "";

                    Console.Write("Enter updated Scholarship Details: ");
                    string updatedDetails = Console.ReadLine() ?? "";

                    UpdateRegisteredApplicant(username, updatedDetails);
                }
                else if (choice == "2")
                {
                    Console.Write("Enter Username to delete: ");
                    string username = Console.ReadLine() ?? "";

                    DeleteRegisteredApplicant(username);
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error viewing registered applicants: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        private static void UpdateRegisteredApplicant(string username, string updatedDetails)
        {
            try
            {
                var applicants = File.ReadAllLines(RegisteredApplicantsFilePath).ToList();
                for (int i = 0; i < applicants.Count; i++)
                {
                    if (applicants[i].StartsWith(username + "|"))
                    {
                        applicants[i] = $"{username}|{updatedDetails}";
                        break;
                    }
                }
                File.WriteAllLines(RegisteredApplicantsFilePath, applicants);
                Console.WriteLine("Applicant updated successfully. Press any key to continue.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error updating registered applicant: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.ReadKey();
        }

        private static void DeleteRegisteredApplicant(string username)
        {
            try
            {
                var applicants = File.ReadAllLines(RegisteredApplicantsFilePath).ToList();
                applicants.RemoveAll(a => a.StartsWith(username + "|"));
                File.WriteAllLines(RegisteredApplicantsFilePath, applicants);
                Console.WriteLine("Applicant deleted successfully. Press any key to continue.");
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error deleting registered applicant: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.ReadKey();
        }

        private static void RemoveRejectedApplication(string username)
        {
            try
            {
                var applications = File.ReadAllLines(ApplicantsFilePath).ToList();
                var rejectedApplications = applications
                    .Where(line => line.StartsWith(username + "|") && line.Split('|')[3] == "Rejected")
                    .ToList();

                if (!rejectedApplications.Any())
                {
                    Console.WriteLine("No rejected applications found for removal. Press any key to continue.");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Rejected Applications:");
                for (int i = 0; i < rejectedApplications.Count; i++)
                {
                    var parts = rejectedApplications[i].Split('|');
                    Console.WriteLine($"{i + 1}. Scholarship ID: {parts[2]}");
                }

                Console.Write("Enter the number of the application to remove: ");
                if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= rejectedApplications.Count)
                {
                    applications.Remove(rejectedApplications[index - 1]); // Remove the selected application
                    File.WriteAllLines(ApplicantsFilePath, applications);
                    Console.WriteLine("Rejected application removed successfully. You can now apply for another scholarship.");
                }
                else
                {
                    Console.WriteLine("Invalid selection. Press any key to continue.");
                }
            }
            catch (IOException ioEx)
            {
                Console.WriteLine($"Error removing rejected application: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.ReadKey();
        }
    }
}