using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ADO.NET
{
    class Program
    {


        static void Main(string[] args)
        {

            #region
            string ConStr = "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=University; Integrated Security=true;";
 
            using (SqlConnection connection = new SqlConnection(ConStr))
            {

                connection.Open();
                Console.WriteLine($"Connection status is {connection.State.ToString()}");
                string query = "";
                string header = "1-Overall Information\n2-Information about students\n3-Average marks\n4-Minimum marks of students\n5-Subjects with minimum marks\n6-Minimum average score\n7-Maximum average score\n8-Minimum average score for Mathematics\n9-Maximum average score for Mathematics\n10-Student count\n11-Average score in groups";
                Console.WriteLine(header);
                int answer = Int32.Parse(Console.ReadLine());
                switch (answer)
                {              
                    case 1:
                        query = @"SELECT G.GROUPNAME,S.FIRSTNAME+S.LASTNAME,SB.NAME,M.MARKS FROM STUDENTS S 
                                    INNER JOIN MARKS M ON S.ID=M.STUDENTID
                                    INNER JOIN GROUPS G ON S.GROUPID=G.ID
                                    INNER JOIN SUBJECTS SB ON M.SUBJECTID=SB.ID";
                        break;
                    case 2:
                        query = @"SELECT FIRSTNAME,LASTNAME FROM STUDENTS";
                        break;
                    case 3:
                        query = @"SELECT S.FIRSTNAME,SB.NAME,AVG(M.MARK) FROM STUDENTS S
                                LEFT JOIN MARKS M ON S.ID=M.STUDENTID
                                LEFT JOIN SUBJECTS SB ON SB.ID=M.SUBJECTID
                                GROUP BY S.FIRSTNAME,SB.NAME";
                        break;
                    case 4:
                        Console.WriteLine("Enter mark:");
                        int score=Int32.Parse(Console.ReadLine());
                        query = $@"SELECT S.FIRSTNAME,S.FIRSTNAME FROM STUDENTS S
                                LEFT JOIN MARKS M ON S.ID=M.STUDENTID
                                WHERE M.MARK<{score}";
                        break;
                    case 5:
                        query = @"SELECT SB.NAME,MIN(M.MARK) FROM SUBJECTS SB
                                LEFT JOIN MARKS M ON M.SUBJECTID=SB.ID
                                GROUP BY SB.NAME";
                        break;
                    case 6:
                        query = @"SELECT MIN(M.MARK) FROM MARKS M
                                WHERE M.MARK IN (SELECT AVG(MARK) FROM MARKS)";
                        break;
                    case 7:
                        query = @"SELECT MAX(M.MARK) FROM MARKS M
                        WHERE M.MARK IN (SELECT AVG(MARK) FROM MARKS)";
                        break;
                    case 8:
                        query = @"SELECT COUNT(S.ID) FROM STUDENTS S
                                INNER JOIN MARKS M ON S.ID=M.STUDENTID
                                WHERE M.MARK IN (SELECT MIN (MS.MARK) FROM MARKS MS WHERE MS.MARK IN (SELECT AVG(MS2.MARK) FROM MARKS MS2) AND MS.SUBJECTID=1)";
                        break;
                    case 9:
                        query = @"SELECT COUNT(S.ID) FROM STUDENTS S
                                INNER JOIN MARKS M ON S.ID=M.STUDENTID
                                WHERE M.MARK IN (SELECT MAX (MS.MARK) FROM MARKS MS WHERE MS.MARK IN (SELECT AVG(MS2.MARK) FROM MARKS MS2) AND MS.SUBJECTID=1)";
                        break;
                    case 10:
                        query = @"SELECT S.GROUPID,COUNT(S.ID) FROM STUDENTS S GROUP BY  S.GROUPID";
                        break;
                    case 11:
                        query = @"SELECT S.GROUPID,AVG(M.MARK)[AVERAGE]FROM STUDENTS S
                                INNER JOIN MARKS M ON M.STUDENTID=S.ID GROUP BY  S.GROUPID";
                        break;
                    default:
                        break;

                }

                try
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    do
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine(reader.GetName(i) + " ");
                        }
                        Console.WriteLine();
                        while (reader.Read())
                        {

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine(reader[i].ToString() + " ");
                            }



                            Console.WriteLine();

                        }
                        Console.WriteLine();
                    } while (reader.NextResult());
                    reader.Close();
                }
                catch (Exception)
                {

                    throw;
                }
                



                #endregion
            }
      
        }
    }
}
