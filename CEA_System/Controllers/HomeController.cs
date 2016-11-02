using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using CEA_System.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Data.Entity.Validation;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using CEA_System.Email;
namespace CEA_System.Controllers
{
    public class HomeController : Controller
    {
        CEAEntities2 context = new CEAEntities2();


        public ActionResult Index()
        {
            ViewBag.LoginMsg = TempData["LoginMsg"];
            ViewBag.ContactMsg = TempData["ContactMessage"];
            ViewBag.RegisterMsg = TempData["RegisterMsg"];
            return View();


        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            //  ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            String firstname = "";
            String lastname = "";
            String email = "";
            String password = "";


            firstname = form["fname"].ToString();
            lastname = form["lname"].ToString();
            email = form["email"].ToString();
            password = form["password"].ToString();
            String cs = ConfigurationManager.ConnectionStrings["CEASYS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open(); // Does the connection with database
                    // ViewBag.Message = "Connection is sucessful";
                    var insertCommand = "INSERT INTO Student_Profile(FirstName, LastName, Email,Password) Values('" + firstname + "','" + lastname + "','" + email + "','" + password + "')";
                    //insertCommand.Parameters.Add("@FirstName", SqlDbType.VarChar, 50).Value = firstName;
                    //cmd.Parameters.Add("@Lastname", SqlDbType.VarChar, 50).Value = lastName;
                    //cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = userName;
                    //cmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = password;
                    //cmd.Parameters.Add("@Age", SqlDbType.Int).Value = age;
                    //cmd.Parameters.Add("@Gender", SqlDbType.VarChar, 50).Value = gender;
                    //cmd.Parameters.Add("@Contact", SqlDbType.VarChar, 50).Value = contact;
                    using (SqlCommand cmd = new SqlCommand(insertCommand, con))
                    {
                        cmd.ExecuteNonQuery();
                        var studentdata = context.Student_Profile.Where(s => s.Email == email).Select(s => s).FirstOrDefault();
                        var addressInfo = new AddressStudent();
                        addressInfo.StudentId = studentdata.StudentId;
                        var citizenshipInfo = new CitizenShipInfo();
                        citizenshipInfo.StudentId = studentdata.StudentId;
                        var academicinfo = new AcademicInfo();
                        academicinfo.StudentId = studentdata.StudentId;
                        var preSchholInfo = new PreviousSchoolInfo();
                        preSchholInfo.StudentId = studentdata.StudentId;
                        var greExam = new ExamInfo();
                        var toeflExam = new ExamInfo();
                        var IeltsExam = new ExamInfo();
                        greExam.StudentId = toeflExam.StudentId = IeltsExam.StudentId = studentdata.StudentId;
                        var workInfo = new WorkInfo();
                        workInfo.StudentId = studentdata.StudentId;

                        context.Entry(addressInfo).State = System.Data.EntityState.Added;
                        context.Entry(citizenshipInfo).State = System.Data.EntityState.Added;
                        context.Entry(academicinfo).State = System.Data.EntityState.Added;
                        context.Entry(preSchholInfo).State = System.Data.EntityState.Added; // 
                        context.Entry(greExam).State = System.Data.EntityState.Added;
                        context.Entry(toeflExam).State = System.Data.EntityState.Added;
                        context.Entry(IeltsExam).State = System.Data.EntityState.Added;
                        context.Entry(workInfo).State = System.Data.EntityState.Added;//

                        context.SaveChanges();


                        TempData["RegisterMsg"] = "Registration has been completed!!";
                    }


                } // end try
                catch (Exception e)  // catches the exception message 
                {

                    TempData["RegisterMsg"] = "Email Already registered.Please try with different email";
                } // end catch
                finally
                {
                    con.Close(); //Closes the connection to database
                }
            }



            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult ContactMe(FormCollection form)
        {
            //  ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            String fullname = "";

            String senderemail = "";
            String message = "";

            fullname = form["fullname"].ToString();
            senderemail = form["senderemail"].ToString();
            message = form["message"].ToString();

            String cs = ConfigurationManager.ConnectionStrings["CEASYS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open(); // Does the connection with database
                    //ViewBag.Message = "Connection is sucessful";
                    var insertCommand = "INSERT INTO ContactMe(FirstName, Email, Message) Values('" + fullname + "','" + senderemail + "','" + message + "')";

                    using (SqlCommand cmd = new SqlCommand(insertCommand, con))
                    {
                        cmd.ExecuteNonQuery();
                        TempData["ContactMessage"] = "We will contact you within 24 hours ";
                    }


                } // end try
                catch (Exception e)  // catches the exception message 
                {
                    TempData["ContactMessage"] = "Maximum message length is 150 characters. ";
                } // end catch
                finally
                {
                    con.Close(); //Closes the connection to database
                }
            }



            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            String username = "";
            String loginpassword = "";

            username = form["username"].ToString();
            loginpassword = form["loginpassword"].ToString();
            //int result=0;

            /// StudentProfile p = new StudentProfile();

            var studentdata = context.Student_Profile.Where(a => a.Email == username && a.Password == loginpassword).Select(a => a).FirstOrDefault();

            //String cs = ConfigurationManager.ConnectionStrings["CEASYS"].ConnectionString;
            //using (SqlConnection con = new SqlConnection(cs))
            //{

            try
            {
                //        con.Open(); // Does the connection with databas
                //         //ViewBag.Message = "Connection is sucessful";
                //        // string SQL = "SELECT * FROM table WHERE fielda='" & myselection & "';"
                //        var selectCommand = "SELECT * FROM Student_Profile where Email='" + username + "' and " + "Password='" + loginpassword+"';";
                //        //fullname + "','" + senderemail + "','" + message + "')";

                //        using (SqlCommand cmd = new SqlCommand(selectCommand, con))
                //        {
                //            SqlDataReader result = cmd.ExecuteReader();

                //            if (result.HasRows)
                //            {
                //                // Read advances to the next row.
                //                while (result.Read())
                //                {p.FirstName =result.GetString( result.GetOrdinal("FirstName"));
                //                p.LastName = result.GetString(result.GetOrdinal("LastName"));
                //                p.Email = result.GetString(result.GetOrdinal("Email"));
                //                p.StudentId = result.GetInt32(result.GetOrdinal("SchoolId"));

                //                }
                if (studentdata == null)
                {
                    TempData["LoginMsg"] = "Username or Password is invalid";
                    return RedirectToAction("Index");

                }

                Session["StudentProfile"] = studentdata;
                //return RedirectToAction("CreateProfile");
                return RedirectToAction("StudentHomePage", new { str = "Welcome to CEA System " + studentdata.FirstName + "!"   });
                

                // }              // result = Convert.ToInt32(cmd.ExecuteScalar());
                //if (result1 != null)
                //{
                //    Session["StudentProfile"]=username;
                //    return RedirectToAction("StudentHome");
                //   // return View("StudentHome");
                //}
                //else
                //{

                //}

                // }

            } // end try
            catch (Exception e)  // catches the exception message 
            {
                ViewBag.Message = "Login Unsuccessful. " + e;
            } // end catch
            finally
            {

                //con.Close(); //Closes the connection to database
            }
            //}
            return RedirectToAction("Index");
        }

        public ActionResult CreateProfile()
        {
            Student_Profile student = Session["StudentProfile"] as Student_Profile;
            // ViewBag.Studentprofile = "";
           
            if (student != null)
            {
                var studentdata = context.Student_Profile.Where(a => a.StudentId == student.StudentId).Select(a => a).FirstOrDefault();
                //   string[] dob = studentdata.DateOfBirth.ToString().Split(' ');
                //  studentdata.DateOfBirth = dob[0].Date;
                return View(studentdata);
            }
            else
            {
                return RedirectToAction("Index");

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProfile(Student_Profile NewStudentdata)
        {
            // var data = Request.Form["studentdata"];
            //  List<ResultAccoLoanDocAnsDto> documentCollection = new List<ResultAccoLoanDocAnsDto>();
            try
            {
                List<HttpPostedFileBase> file = new List<HttpPostedFileBase>();

                if (Request.Files.Count > 0)
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  
                        file.Add(files[i]);

                    }
                }

                var path2 = "";
                string MaterialName = string.Empty;
                foreach (var item in file)
                {
                    if (item.ContentLength > 0)
                    {
                        var studentfiledoc = new SupportingDoc();
                        studentfiledoc.mimeType = item.ContentType;
                        Stream fileStream = item.InputStream;
                        // string fileName = item.FileName;
                        studentfiledoc.filelength = item.ContentLength.ToString();
                        byte[] fileData = new byte[item.ContentLength];
                        fileStream.Read(fileData, 0, item.ContentLength);
                        studentfiledoc.TypeofDoc = item.FileName;////////////////////////////////////////////////////
                        //var new_path = "~/StudentDoc/";
                        //path2 = Path.Combine(Server.MapPath(new_path), NewStudentdata.StudentId+"_"+item.FileName);
                        //item.SaveAs(path2);

                        studentfiledoc.StudentId = NewStudentdata.StudentId;
                        studentfiledoc.ServerURL = "~/StudentDoc/" + NewStudentdata.StudentId + "_" + item.FileName;
                        //  studentfiledoc.TypeofDoc = item.ContentLength;
                        studentfiledoc.filecontent = fileData;
                        context.Entry(studentfiledoc).State = System.Data.EntityState.Added;
                        context.SaveChanges();
                    }

                }
                if (NewStudentdata.Filefordelete != null)
                {
                    string[] filenumber = NewStudentdata.Filefordelete.Split('_');
                    var filestotal = context.SupportingDocs.Where(a => a.StudentId == NewStudentdata.StudentId).Select(a => a).ToList();
                    foreach (var item in filestotal)
                    {
                        for (int i = 1; i < filenumber.Length; i++)
                        {
                            if (filenumber[i] != "" || filenumber[i] != " ")
                            {
                                if (item.SupportingDocId == int.Parse(filenumber[i]))
                                {
                                    context.Entry(item).State = System.Data.EntityState.Deleted;

                                    break;
                                }
                            }

                        }
                    }
                }
                NewStudentdata.Filefordelete = null;
                NewStudentdata.WorkInfoes.ElementAtOrDefault(0).Description = NewStudentdata.WorkInfoes.ElementAtOrDefault(0).Description.Trim();
                NewStudentdata.WorkInfoes.ElementAtOrDefault(0).Achievement = NewStudentdata.WorkInfoes.ElementAtOrDefault(0).Achievement.Trim();

                //  var studentdataobj = JsonConvert.DeserializeObject<Student_Profile>(data);
                NewStudentdata.AddressStudents.ElementAtOrDefault(0).StudentId = NewStudentdata.StudentId;
                NewStudentdata.CitizenShipInfoes.ElementAtOrDefault(0).StudentId = NewStudentdata.StudentId;
                NewStudentdata.AcademicInfoes.ElementAtOrDefault(0).StudentId = NewStudentdata.StudentId;
                NewStudentdata.PreviousSchoolInfoes.ElementAtOrDefault(0).StudentId = NewStudentdata.StudentId;
                NewStudentdata.ExamInfoes.ElementAtOrDefault(0).StudentId = NewStudentdata.StudentId;
                NewStudentdata.ExamInfoes.ElementAtOrDefault(1).StudentId = NewStudentdata.StudentId;
                NewStudentdata.ExamInfoes.ElementAtOrDefault(2).StudentId = NewStudentdata.StudentId;
                NewStudentdata.WorkInfoes.ElementAtOrDefault(0).StudentId = NewStudentdata.StudentId;


                // context.Entry(NewStudentdata.AddressStudents.ElementAtOrDefault(0)).State = System.Data.EntityState.Modified;
                context.Entry(NewStudentdata).State = System.Data.EntityState.Modified;
                context.Entry(NewStudentdata.AddressStudents.ElementAtOrDefault(0)).State = System.Data.EntityState.Modified;
                context.Entry(NewStudentdata.CitizenShipInfoes.ElementAtOrDefault(0)).State = System.Data.EntityState.Modified;
                context.Entry(NewStudentdata.AcademicInfoes.ElementAtOrDefault(0)).State = System.Data.EntityState.Modified;
                context.Entry(NewStudentdata.PreviousSchoolInfoes.ElementAtOrDefault(0)).State = System.Data.EntityState.Modified;
                context.Entry(NewStudentdata.ExamInfoes.ElementAtOrDefault(0)).State = System.Data.EntityState.Modified;
                context.Entry(NewStudentdata.ExamInfoes.ElementAtOrDefault(1)).State = System.Data.EntityState.Modified;
                context.Entry(NewStudentdata.ExamInfoes.ElementAtOrDefault(2)).State = System.Data.EntityState.Modified;
                context.Entry(NewStudentdata.WorkInfoes.ElementAtOrDefault(0)).State = System.Data.EntityState.Modified;


                context.SaveChanges();

              // string[] filenumber= NewStudentdata.Filefordelete.Split('_');
             //   filenumber[filenumber.Length] = "0";
             



               // context.SaveChanges();
                // return View(NewStudentdata);
                //   return Json(NewStudentdata, JsonRequestBehavior.AllowGet);
                return RedirectToAction("StudentHomePage", new { str = "Your Profile has been be created successfully" });


            }
            catch (DbEntityValidationException dbEx)
            {
                StringBuilder str1 = new StringBuilder();
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        str1.Append(validationErrors.Entry.Entity.GetType().FullName + " " + validationError.PropertyName + " " + validationError.ErrorMessage);
                    }
                }

                ViewBag.msg = "Please enter all Fields ";

            }


            //  ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            // Student_Profile student = Session["StudentProfile"] as Student_Profile;

            //var studentdata = context.Student_Profile.Where(a =>a.StudentId==student.StudentId).Select(a => a).FirstOrDefault();
            // studentdata = NewStudentdata;

            // context.Student_Profile.(NewStudentdata);


            // var studentdata=context.Student_Profile.Select(a=>a);
            /*String firstname = "";
            String Middlename = "";
            String Lastname = "";
            String Phonenumber = "";
            DateTime date;
            String Gender = "";
            String SSN = "";
            String COB = "";


           student.Firstname= firstname = form["fname"].ToString();
           student.MiddleName= Middlename = form["mname"].ToString();
           student.LastName= Lastname = form["lname"].ToString();
           student.PhoneNumber= Phonenumber = form["phname"].ToString();
           student.DateOfBirth= date = Convert.ToDateTime(form["DOB"].ToString());
           student.Gender= Gender = form["gender"].ToString();
           student.SSN= SSN = form["ssn"].ToString();
           student.CountryofBirth= COB = form["COB"].ToString();


            String cs = ConfigurationManager.ConnectionStrings["CEASYS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open(); // Does the connection with database
                    //ViewBag.Message = "Connection is sucessful";
                    var insertCommand = "UPDATE Student_Profile SET FirstName='" + firstname + "', MiddleName='" + Middlename + "', LastName='" + Lastname + "', PhoneNumber='" + Phonenumber + "', DateofBirth='" + date + "', Gender='" + Gender + "',SSN='" + SSN + "', CountryofBirth='" + COB + "' where Email='" + student.Email + "';";

                    using (SqlCommand cmd = new SqlCommand(insertCommand, con))
                    {
                        cmd.ExecuteNonQuery();
                        ViewBag.Studentprofile = "You data has been saved";
                    }


                } // end try
                catch (Exception e)  // catches the exception message 
                {
                    //ViewBag.Studentprofile = "Please try later!!";
                } // end catch
                finally
                {
                    con.Close(); //Closes the connection to database
                }*/
            return View(NewStudentdata);

            //   }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public ActionResult SchoolCompareGet()
        {
            ViewBag.tableStatus = "none";
            List<SchoolCompare> compare = new List<SchoolCompare>();
            SchoolCompare school = new SchoolCompare();
            //   SchoolC school = new SchoolCompare();

            String cs = ConfigurationManager.ConnectionStrings["CEASYS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {

                try
                {
                    con.Open(); // Does the connection with databas
                    var selectCommand = "SELECT SchoolId, SchoolName FROM SchoolData";
                    using (SqlCommand cmd = new SqlCommand(selectCommand, con))
                    {
                        SqlDataReader result = cmd.ExecuteReader();

                        if (result.HasRows)
                        {
                            // Read advances to the next row.
                            while (result.Read())
                            {
                                school.SchoolId = result.GetInt32(result.GetOrdinal("SchoolId"));

                                school.SchoolName = result.GetString(result.GetOrdinal("SchoolName"));
                                compare.Add(new SchoolCompare { SchoolId = school.SchoolId, SchoolName = school.SchoolName });

                            }
                            // result.NextResult();

                        }
                        Session["SchoolNames"] = compare;
                        return View(compare);

                    }

                } // end try
                catch (Exception e)  // catches the exception message 
                {
                    ViewBag.SchoolMessage = "School data unavailable" + e;
                } // end catch
                finally
                {

                    con.Close();
                    //Closes the connection to database
                }
            }
            return View();



        }

        [HttpPost]
        public ActionResult SchoolCompareGet(string[] schoolname)
        {
            ViewBag.tableStatus = "inline";
            SqlDataReader result;
            List<SchoolCompare> SchoolWholeData = new List<SchoolCompare>();
            SchoolCompare s = new SchoolCompare();
            String cs = ConfigurationManager.ConnectionStrings["CEASYS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {

                try
                {
                    con.Open(); // Does the connection with databas
                    foreach (var item in schoolname)
                    {
                        int schoolid = int.Parse(item);


                        var selectCommand = "SELECT * FROM SchoolData where SchoolId=" + schoolid;
                        using (SqlCommand cmd = new SqlCommand(selectCommand, con))
                        {
                            result = cmd.ExecuteReader();

                            if (result.HasRows)
                            {
                                // Read advances to the next row.
                                while (result.Read())
                                {
                                    s.SchoolId = result.GetInt32(result.GetOrdinal("SchoolId"));
                                    s.AddressSchool = result.GetString(result.GetOrdinal("AddressSchool"));
                                    s.City = result.GetString(result.GetOrdinal("City"));
                                    s.SchoolCode = result.GetString(result.GetOrdinal("SchoolCode"));
                                    s.WebSite = result.GetString(result.GetOrdinal("WebSite"));
                                    s.SchoolType = result.GetString(result.GetOrdinal("SchoolType"));
                                    s.TutionFee = result.GetString(result.GetOrdinal("TutionFee"));
                                    s.GraduateRate = result.GetString(result.GetOrdinal("GraduateRate"));
                                    s.RetentionRate = result.GetString(result.GetOrdinal("RetentionRate"));
                                    s.BestProgram = result.GetString(result.GetOrdinal("BestProgram"));
                                    s.ReviewLink = result.GetString(result.GetOrdinal("ReviewLink"));
                                    s.AcceptanceRate = result.GetString(result.GetOrdinal("AcceptanceRate"));
                                    s.Comment = result.GetString(result.GetOrdinal("Comment"));
                                    s.EmailAddress = result.GetString(result.GetOrdinal("EmailAddress"));
                                    s.CourseAvailable = result.GetString(result.GetOrdinal("CourseAvailable"));
                                    s.SchoolName = result.GetString(result.GetOrdinal("SchoolName"));


                                    SchoolWholeData.Add(new SchoolCompare
                                    {
                                        SchoolId = s.SchoolId,
                                        AddressSchool = s.AddressSchool,
                                        City = s.City,
                                        SchoolCode = s.SchoolCode,
                                        WebSite = s.WebSite,
                                        SchoolType = s.SchoolType,
                                        TutionFee = s.TutionFee,
                                        GraduateRate = s.GraduateRate,
                                        RetentionRate = s.RetentionRate,
                                        BestProgram = s.BestProgram,
                                        ReviewLink = s.ReviewLink,
                                        AcceptanceRate = s.AcceptanceRate,
                                        Comment = s.Comment,
                                        EmailAddress = s.EmailAddress,
                                        CourseAvailable = s.CourseAvailable,
                                        SchoolName = s.SchoolName
                                    }); // end of Add list


                                } // end while
                                // result.NextResult();

                            } // end if
                            result.Close();

                        } // end usisng

                    }//foreach
                    return View(SchoolWholeData);

                } // end try
                catch (Exception e)  // catches the exception message 
                {
                    ViewBag.SchoolMessage = "School data unavailable" + e;
                } // end catch
                finally
                {

                    con.Close();
                    //Closes the connection to database
                } // end finally
            } // end using



            return View();
        } // end schoolcompareget

        public ActionResult StudentHomePage(string str)
        {
            ViewBag.msg = str;
            return View();
        }

        public ActionResult ApplyCollege()
        {

            var schooldata = context.SchoolDatas.Select(a => a).ToList();
            ViewData["SchoolName"] = schooldata;
            ApplyCollege data = new ApplyCollege();
            Student_Profile student = Session["StudentProfile"] as Student_Profile;
            data.StudentId = student.StudentId;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyCollege(ApplyCollege AppliedCollege, FormCollection form1)
        {
            

            AppliedCollege.DateApplied = DateTime.Now;
            context.Entry(AppliedCollege).State = System.Data.EntityState.Added;
            context.SaveChanges();
            var Filepath = Server.MapPath("pdf") + "\\" + "First PDF document.pdf";
            var Filepath2 = Server.MapPath("pdf") + "\\";

            //Process for creating pdf
            Student_Profile student = Session["StudentProfile"] as Student_Profile;
            //string[] filescollection = Directory.GetFiles(Filepath2, "First PDF document.pdf", SearchOption.AllDirectories);

            //foreach (var item1 in filescollection)
            //{
               
            //    System.IO.File.Delete(item1);
            //}
            //MemoryStream fs = new MemoryStream();
            if (System.IO.File.Exists(Filepath))
            {
                System.IO.File.Delete(Filepath);
            }

            System.IO.FileStream fs = new FileStream(Filepath, FileMode.Create);
           
            var doc1 = new Document();
            PdfWriter writer = PdfWriter.GetInstance(doc1, fs);   
            doc1.Open();
            doc1.Add(new Paragraph("STUDENT APPLICATION FORM "));
            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Student Data"));
            cell.Colspan = 2;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);
            table.AddCell("First Name");
            table.AddCell(student.FirstName);
            table.AddCell(" Middle Name");
            table.AddCell(student.MiddleName);
            table.AddCell("Last Name");
            table.AddCell(student.LastName);
            table.AddCell("Phone Number");
            table.AddCell(student.PhoneNumber);
            table.AddCell("DateofBirth ");
            table.AddCell(student.DateOfBirth.Value.ToShortDateString());
            table.AddCell("Gender ");
            table.AddCell(student.Gender);
            table.AddCell("SSN");
            table.AddCell(student.SSN);
            table.AddCell("Email");
            table.AddCell(student.Email);
            table.AddCell("Country of Birth");
            table.AddCell(student.CountryofBirth);
            

           
            PdfPCell cell1 = new PdfPCell(new Phrase("Address Informtion"));
            cell1.Colspan = 2;
            cell1.HorizontalAlignment = 1;
            table.AddCell(cell1);
            table.AddCell("Street Address");
            table.AddCell(student.AddressStudents.ElementAtOrDefault(0).AptStreet);
            table.AddCell("City");
            table.AddCell(student.AddressStudents.ElementAtOrDefault(0).City);
            table.AddCell("State");
            table.AddCell(student.AddressStudents.ElementAtOrDefault(0).State);
            table.AddCell("ZipCode");
            table.AddCell(student.AddressStudents.ElementAtOrDefault(0).ZipCode);
            table.AddCell("Country");
            table.AddCell(student.AddressStudents.ElementAtOrDefault(0).Country);
            table.AddCell("Address Type");
            var x = student.AddressStudents.ElementAtOrDefault(0).Type;
            if (x == "1")
            {
                table.AddCell("Permanent");
            }
            else { table.AddCell("Temporary"); }
            


            PdfPCell cell2 = new PdfPCell(new Phrase("Citizenship Informtion"));
            cell2.Colspan = 2;
            cell2.HorizontalAlignment = 1;
            table.AddCell(cell2);
            table.AddCell("Country of Citizenship");
            table.AddCell(student.CitizenShipInfoes.ElementAtOrDefault(0).CountryOfCitizenship);
            table.AddCell("Ethnicity");
            table.AddCell(student.CitizenShipInfoes.ElementAtOrDefault(0).Ethnicity);
            table.AddCell("I20 required ");
            table.AddCell(student.CitizenShipInfoes.ElementAtOrDefault(0).I20Required);
            table.AddCell("Visa Status ");
            table.AddCell(student.CitizenShipInfoes.ElementAtOrDefault(0).VisaStatus);
            table.AddCell("Current Location ");
            table.AddCell(student.CitizenShipInfoes.ElementAtOrDefault(0).CurrentLocation);
            table.AddCell("Vetran Status ");
            table.AddCell(student.CitizenShipInfoes.ElementAtOrDefault(0).VeteranStatus);
           


           
            PdfPCell cell3 = new PdfPCell(new Phrase("Information regarding course enrollement"));
            cell3.Colspan = 2;
            cell3.HorizontalAlignment = 1;
            table.AddCell(cell3);
            table.AddCell("Admission Type");
            table.AddCell(student.AcademicInfoes.ElementAtOrDefault(0).AdmissionType);
            table.AddCell("Semester");
            table.AddCell(student.AcademicInfoes.ElementAtOrDefault(0).Semester);
            table.AddCell("Application Year");
            table.AddCell(student.AcademicInfoes.ElementAtOrDefault(0).ApplicationYear);
            table.AddCell("Major");
            table.AddCell(student.AcademicInfoes.ElementAtOrDefault(0).Major);
         

            PdfPTable table4 = new PdfPTable(3);
            PdfPCell cell4 = new PdfPCell(new Phrase("Previous School information"));
            cell4.Colspan = 2;
            cell4.HorizontalAlignment = 1;
            table.AddCell(cell4);
            table.AddCell("Previous School Name");
            table.AddCell(student.PreviousSchoolInfoes.ElementAtOrDefault(0).SchoolName);
            table.AddCell("Previous School Start Date");
            table.AddCell(student.PreviousSchoolInfoes.ElementAtOrDefault(0).DateFrom.Value.ToShortDateString());
            table.AddCell("Previous School End Date");
            table.AddCell(student.PreviousSchoolInfoes.ElementAtOrDefault(0).DateTo.Value.ToShortDateString());
            table.AddCell("Degree");
            table.AddCell(student.PreviousSchoolInfoes.ElementAtOrDefault(0).DegreeName);
            table.AddCell("Major");
            table.AddCell(student.PreviousSchoolInfoes.ElementAtOrDefault(0).Major);
            table.AddCell("School Address");
            table.AddCell(student.PreviousSchoolInfoes.ElementAtOrDefault(0).Address);

            PdfPCell cell5 = new PdfPCell(new Phrase("Exam Information"));
            cell5.Colspan = 2;
            cell5.HorizontalAlignment = 1;
            table.AddCell(cell5);
            table.AddCell("GRE TAKEN ?");
            var y = student.ExamInfoes.ElementAtOrDefault(0).ExamType;
                if(y == null){
                table.AddCell("NO");
                }
                else{
                    table.AddCell("YES");
                    table.AddCell("GRE SCORE");
                    table.AddCell(student.ExamInfoes.ElementAtOrDefault(0).Score);
                    table.AddCell("GRE DATE");
                    table.AddCell(student.ExamInfoes.ElementAtOrDefault(0).DateOfExam.Value.ToShortDateString());                   
                
                 }
                table.AddCell("TOEFL TAKEN ?");
                var z = student.ExamInfoes.ElementAtOrDefault(1).ExamType;
                if (z == null)
                {
                    table.AddCell("NO");
                }
                else
                {
                    table.AddCell("YES");
                    table.AddCell("TOEFL SCORE");
                    table.AddCell(student.ExamInfoes.ElementAtOrDefault(1).Score);
                    table.AddCell("TOEFL DATE");
                    table.AddCell(student.ExamInfoes.ElementAtOrDefault(1).DateOfExam.Value.ToShortDateString());

                }
                table.AddCell("IELTS TAKEN ?");
                var p = student.ExamInfoes.ElementAtOrDefault(2).ExamType;
                if (p == null)
                {
                    table.AddCell("NO");
                }
                else
                {
                    table.AddCell("YES");
                    table.AddCell("IELTS SCORE");
                    table.AddCell(student.ExamInfoes.ElementAtOrDefault(2).Score);
                    table.AddCell("IELTS DATE");
                    table.AddCell(student.ExamInfoes.ElementAtOrDefault(2).DateOfExam.Value.ToShortDateString());

                }


                PdfPCell cell6 = new PdfPCell(new Phrase("Work Experience"));
                cell6.Colspan = 2;
                cell6.HorizontalAlignment = 1;
                table.AddCell(cell6);
                table.AddCell("Work Experience");
                var r = student.WorkInfoes.ElementAtOrDefault(0).CompanyName;
                if (r == null)
                {

                    table.AddCell("NO");
                }
                else {
                    table.AddCell("YES");
                    table.AddCell("Company name");
                    table.AddCell(student.WorkInfoes.ElementAtOrDefault(0).CompanyName);
                    table.AddCell("Start date");
                    table.AddCell(student.WorkInfoes.ElementAtOrDefault(0).FromDate.Value.ToShortDateString());
                    table.AddCell("End date");
                    table.AddCell(student.WorkInfoes.ElementAtOrDefault(0).ToDate.Value.ToShortDateString());
                    table.AddCell("Description");
                    table.AddCell(student.WorkInfoes.ElementAtOrDefault(0).Description);
                    table.AddCell("Achievement");
                    table.AddCell(student.WorkInfoes.ElementAtOrDefault(0).Achievement);
                }
            
            doc1.Add(table);
            doc1.Close();            
            fs.Close();
            fs.Dispose();
            writer.Close();
            writer.Dispose();
               

            EmailHelper cs = new EmailHelper();
            string result = cs.SendContactUsEmail(student.StudentId, 0, Filepath2, Filepath);          

            
            return RedirectToAction("StudentHomePage", new { str = "Your application has been submitted to" + AppliedCollege.SchoolName + " sucessfully!! " });

        }

        public ActionResult History()
        {
            var schooldata = context.ApplyColleges.Select(a => a).ToList();
            ViewData["History"] = schooldata;
            List<ApplyCollege> history = new List<ApplyCollege>();
            Student_Profile student = Session["StudentProfile"] as Student_Profile;
            var x = schooldata.Count();
            for (int i = 0; i < x; i++)
            {
                if (schooldata.ElementAtOrDefault(i).StudentId == student.StudentId)
                {
                    ApplyCollege his = new ApplyCollege();
                    his.ApplyCollegeId = schooldata.ElementAtOrDefault(i).ApplyCollegeId;
                    his.StudentId = schooldata.ElementAtOrDefault(i).StudentId;
                    his.SchoolName = schooldata.ElementAtOrDefault(i).SchoolName;
                    his.CollegeEmail = schooldata.ElementAtOrDefault(i).CollegeEmail;
                    his.CourseApplied = schooldata.ElementAtOrDefault(i).CourseApplied;
                    his.Semester = schooldata.ElementAtOrDefault(i).Semester;
                    his.DateApplied = schooldata.ElementAtOrDefault(i).DateApplied;
                    history.Add(his); 
                }


            }
            return View(history);
        }



        public void GetDocumentById(int docId, int StudentId)
        {
            var document = context.SupportingDocs.Where(a => a.SupportingDocId == docId && a.StudentId == StudentId).Select(a => a).FirstOrDefault();

            var response = HttpContext.Response;
            response.ContentType = "application/octet-stream";
            // response.AppendHeader("Content-Disposition", "attachment; filename=" + document.FileName);
            response.AppendHeader("Content-Disposition", "attachment; filename=" + document.TypeofDoc);
            response.OutputStream.Write(document.filecontent, 0, document.filecontent.Length);
            response.End();
        }

        public ActionResult ForgotPassword(FormCollection form)
        {
            var email = form["forgotemail"];
            var studentdata = context.Student_Profile.Where(a => a.Email == email).Select(a => a).FirstOrDefault();
            EmailHelper e = new EmailHelper();
            e.SendPasswordEmail(studentdata.Password, studentdata.Email);
            return View("Index");
        }

        public ActionResult Review()
        {
            var schooldata = context.SchoolDatas.Select(a => a).ToList();
            var reviewdata = context.Reviews.Select(a => a).ToList();
            ViewData["SchoolName"] = schooldata;
            return View(reviewdata);
            

        }
        [HttpPost]
        public ActionResult Review(FormCollection form)
        {
            var schoolname = form["SchoolName"];
            var studnettext = form["Review1"];
            Student_Profile student = Session["StudentProfile"] as Student_Profile;
            Review studetnreview = new Review();
            studetnreview.Review1 = studnettext.Trim();
            studetnreview.SchoolName = schoolname;
            studetnreview.Studentname = student.FirstName + " " + student.LastName;
            context.Entry(studetnreview).State = System.Data.EntityState.Added;
            context.SaveChanges();
            return RedirectToAction("StudentHomePage", new { str = "Your review has been submitted. Thanks for you feedback"});

        }



    }
}
