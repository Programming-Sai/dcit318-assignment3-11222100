using System;
using System.Collections.Generic;
using System.IO;

namespace GradeProcessor
{
    // Custom Exceptions
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // Student Class
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Score { get; set; }

        public string GetGrade()
        {
            if (Score >= 80) return "A";
            if (Score >= 70) return "B";
            if (Score >= 60) return "C";
            if (Score >= 50) return "D";
            return "F";
        }
    }

    // Processor Class
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            List<Student> students = new List<Student>();

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string line;
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    string[] parts = line.Split(',');

                    if (parts.Length != 3)
                    {
                        throw new MissingFieldException($"Line {lineNumber}: Expected 3 fields, found {parts.Length}.");
                    }

                    try
                    {
                        int id = int.Parse(parts[0].Trim());
                        string name = parts[1].Trim();
                        int score = int.Parse(parts[2].Trim());

                        students.Add(new Student
                        {
                            Id = id,
                            FullName = name,
                            Score = score
                        });
                    }
                    catch (FormatException)
                    {
                        throw new InvalidScoreFormatException($"Line {lineNumber}: Score must be an integer.");
                    }
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (Student student in students)
                {
                    string summary = $"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}";
                    writer.WriteLine(summary);
                }
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main()
        {
            string projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            string inputPath = Path.Combine(projectRoot, "input.txt");
            string outputPath = Path.Combine(projectRoot, "report.txt");
            Console.WriteLine($"Looking for file at: {Path.GetFullPath(inputPath)}");

            try
            {
                StudentResultProcessor processor = new StudentResultProcessor();
                List<Student> students = processor.ReadStudentsFromFile(inputPath);
                processor.WriteReportToFile(students, outputPath);
                Console.WriteLine("Summary report generated successfully.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: Input file not found.");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
