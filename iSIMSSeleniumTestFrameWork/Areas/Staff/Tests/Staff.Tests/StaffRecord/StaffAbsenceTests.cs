using Microsoft.VisualStudio.TestTools.UnitTesting;
using Staff.POM.Components.Staff;
using Staff.POM.Helper;
using System;
using System.Linq;
using SeSugar.Automation;
using SeSugar.Data;
using WebDriverRunner.internals;
using Selene.Support.Attributes;

namespace Staff.Tests.StaffRecord
{
    [TestClass]
    public class StaffAbsenceTests
    {
        #region MS Unit Testing support
        public TestContext TestContext { get; set; }
        [TestInitialize]
        public void Init()
        {
            TestRunner.VSSeleniumTest.Init(this, TestContext);
        }
        [TestCleanup]
        public void Cleanup()
        {
            TestRunner.VSSeleniumTest.Cleanup(TestContext);
        }
        #endregion
        #region Private Parameters

        private readonly int tenantID = SeSugar.Environment.Settings.TenantId;
        private const string dateTimeFormat = "dd/MM/yyy HH:mm";

        #region Staff Absence

        private readonly DateTime startDate = DateTime.Today.AddDays(-1);
        private readonly DateTime endDate = DateTime.Today;
        private const string apr = "Half Pay Rate";
        private const string at = "Unauthorised absence";
        private const string ic = "Allergy";
        private const string workingDaysLost = "1.0000";
        private const string workingHoursLost = "6.5000";
        private const string notes = "Test Notes";

        #endregion

        #region Staff Absence Certificate

        private const string certificateAdvice = "May be fit for work, taking into account the doctor's advice";
        private const string nffw = "Not fit for work";
        private const string selfSignatoryType = "Self";
        private const string doctorSignatoryType = "Doctor";
        private const string signedBy = "Mr Test";

        #endregion

        #endregion

        #region Staff Absence

        [TestMethod]
        [ChromeUiTest("StaffAbsence", "P1", "Create")]
        public void Create_new_absence_to_existing_staff_record_as_PO()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            using (new DataSetup(GetStaffRecord(id, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(id);
                staff.SelectAbsencesTab();
                staff.ClickAddStaffAbsence();

                StaffAbsenceDialog absence = new StaffAbsenceDialog
                {
                    StartDate = startDate.ToString(),
                    EndDate = endDate.ToString(),
                    ExpectedReturnDate = endDate.ToString(),
                    ActualReturnDate = endDate.ToString(),
                    AbsencePayRate = apr,
                    IllnessCategory = ic,
                    PayrollAbsenceCategory = pacDescription,
                    AbsenceType = at,
                    WorkingDaysLost = workingDaysLost,
                    WorkingHoursLost = workingHoursLost,
                    AnnualLeave = true,
                    IndustrialInjury = true,
                    SSPExclusion = true,
                    Notes = notes
                };

                absence.ClickOk();
                staff.SaveStaff();
                staff = LoadStaff(id);
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                absence = new StaffAbsenceDialog();

                //Assert              
                Assert.AreEqual(startDate.ToString(dateTimeFormat), absence.StartDate);
                Assert.AreEqual(endDate.ToString(dateTimeFormat), absence.EndDate);
                Assert.AreEqual(endDate.ToString(dateTimeFormat), absence.ExpectedReturnDate);
                Assert.AreEqual(endDate.ToString(dateTimeFormat), absence.ActualReturnDate);
                Assert.AreEqual(apr, absence.AbsencePayRate);
                Assert.AreEqual(at, absence.AbsenceType);
                Assert.AreEqual(ic, absence.IllnessCategory);
                Assert.AreEqual(pacDescription, absence.PayrollAbsenceCategory);
                Assert.AreEqual(workingDaysLost, absence.WorkingDaysLost);
                Assert.AreEqual(workingHoursLost, absence.WorkingHoursLost);
                Assert.AreEqual(true, absence.AnnualLeave);
                Assert.AreEqual(true, absence.IndustrialInjury);
                Assert.AreEqual(true, absence.SSPExclusion);
                Assert.AreEqual(notes, absence.Notes);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAbsence", "P1", "Read")]
        public void Read_absence_from_staff_record_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                var absence = new StaffAbsenceDialog();

                //Assert
                Assert.AreEqual(startDate.ToString(dateTimeFormat), absence.StartDate);
                Assert.AreEqual(endDate.ToString(dateTimeFormat), absence.EndDate);
                Assert.AreEqual(endDate.ToString(dateTimeFormat), absence.ExpectedReturnDate);
                Assert.AreEqual(endDate.ToString(dateTimeFormat), absence.ActualReturnDate);
                Assert.AreEqual(apr, absence.AbsencePayRate);
                Assert.AreEqual(at, absence.AbsenceType);
                Assert.AreEqual(ic, absence.IllnessCategory);
                Assert.AreEqual(pacDescription, absence.PayrollAbsenceCategory);
                Assert.AreEqual(workingDaysLost, absence.WorkingDaysLost);
                Assert.AreEqual(workingHoursLost, absence.WorkingHoursLost);
                Assert.AreEqual(true, absence.AnnualLeave);
                Assert.AreEqual(true, absence.IndustrialInjury);
                Assert.AreEqual(true, absence.SSPExclusion);
                Assert.AreEqual(notes, absence.Notes);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAbsence", "P1", "Update")]
        public void Update_absence_in_staff_record_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            DateTime newStartDate = DateTime.Today.AddDays(1);
            DateTime newEndDate = DateTime.Today.AddDays(3);

            const string newApr = "Nil Pay Rate";
            const string newAt = "Unpaid, authorised absence";
            const string newIc = "Blood Condition";
            const string newWorkingDaysLost = "2.0000";
            const string newWorkingHoursLost = "13.0000";
            const string newNotes = "Still Test Notes";

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();

                StaffAbsenceDialog absence = new StaffAbsenceDialog
                {
                    StartDate = newStartDate.ToString(),
                    EndDate = newEndDate.ToString(),
                    ExpectedReturnDate = newEndDate.ToString(),
                    ActualReturnDate = newEndDate.ToString(),
                    AbsencePayRate = newApr,
                    IllnessCategory = newIc,
                    AbsenceType = newAt,
                    WorkingDaysLost = newWorkingDaysLost,
                    WorkingHoursLost = newWorkingHoursLost,
                    AnnualLeave = false,
                    IndustrialInjury = false,
                    SSPExclusion = false,
                    Notes = newNotes
                };

                absence.ClickOk();
                staff.SaveStaff();
                staff = LoadStaff(staffId);
                gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                absence = new StaffAbsenceDialog();

                //Assert
                Assert.AreEqual(newStartDate.ToString(dateTimeFormat), absence.StartDate);
                Assert.AreEqual(newEndDate.ToString(dateTimeFormat), absence.EndDate);
                Assert.AreEqual(newEndDate.ToString(dateTimeFormat), absence.ExpectedReturnDate);
                Assert.AreEqual(newEndDate.ToString(dateTimeFormat), absence.ActualReturnDate);
                Assert.AreEqual(newApr, absence.AbsencePayRate);
                Assert.AreEqual(newAt, absence.AbsenceType);
                Assert.AreEqual(newIc, absence.IllnessCategory);
                Assert.AreEqual(newWorkingDaysLost, absence.WorkingDaysLost);
                Assert.AreEqual(newWorkingHoursLost, absence.WorkingHoursLost);
                Assert.AreEqual(false, absence.AnnualLeave);
                Assert.AreEqual(false, absence.IndustrialInjury);
                Assert.AreEqual(false, absence.SSPExclusion);
                Assert.AreEqual(newNotes, absence.Notes);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAbsence", "P1", "Delete")]
        public void Delete_absence_from_staff_record_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.DeleteRow();
                staff.SaveStaff();

                staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                gridRow = staff.Absences.Rows.FirstOrDefault();

                //Assert
                Assert.AreEqual(null, gridRow);
            }
        }

        #endregion

        #region Staff Absence Certificate

        [TestMethod]
        [ChromeUiTest("StaffAbsenceCertificate", "P1", "Create", "Doctor")]
        public void Create_new_doctors_absence_certification_to_existing_absence_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                var absence = new StaffAbsenceDialog();
                absence.ClickAddDoctorCertification();

                var absenceCertificate = new StaffAbsenceCertificateDialog
                {
                    DateRecieved = startDate.ToShortDateString(),
                    DateSigned = startDate.ToShortDateString(),
                    SignedBy = signedBy,
                    StartDate = startDate.ToShortDateString(),
                    EndDate = endDate.ToShortDateString(),
                    CertificateAdvice = certificateAdvice,
                    PhasedReturn = true,
                    AmendedDuties = true,
                    AlteredHours = true,
                    WorkplaceAdaptations = true,
                    IsReAssessmentRequired = true
                };


                absenceCertificate.ClickOk();
                absence.ClickOk();
                staff.SaveStaff();
                staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                absence = new StaffAbsenceDialog();
                var acGridRow = absence.AbsenceCertificates.Rows[0];
                acGridRow.ClickEdit();
                absenceCertificate = new StaffAbsenceCertificateDialog();

                //Assert
                Assert.AreEqual(doctorSignatoryType, absenceCertificate.SignatoryType);
                Assert.AreEqual("2", absenceCertificate.Duration);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.DateRecieved);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.DateSigned);
                Assert.AreEqual(signedBy, absenceCertificate.SignedBy);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.StartDate);
                Assert.AreEqual(endDate.ToShortDateString(), absenceCertificate.EndDate);
                Assert.AreEqual(certificateAdvice, absenceCertificate.CertificateAdvice);
                Assert.AreEqual(true, absenceCertificate.PhasedReturn);
                Assert.AreEqual(true, absenceCertificate.AmendedDuties);
                Assert.AreEqual(true, absenceCertificate.AlteredHours);
                Assert.AreEqual(true, absenceCertificate.WorkplaceAdaptations);
                Assert.AreEqual(true, absenceCertificate.IsReAssessmentRequired);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAbsenceCertificate", "P1", "Create", "Self")]
        public void Create_new_self_absence_certification_to_existing_absence_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                var absence = new StaffAbsenceDialog();
                absence.ClickAddSelfCertification();

                var absenceCertificate = new StaffAbsenceCertificateDialog
                {
                    DateRecieved = startDate.ToShortDateString(),
                    DateSigned = startDate.ToShortDateString(),
                    SignedBy = signedBy,
                };

                absenceCertificate.ClickOk();
                absence.ClickOk();
                staff.SaveStaff();
                staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                absence = new StaffAbsenceDialog();
                var acGridRow = absence.AbsenceCertificates.Rows[0];
                acGridRow.ClickEdit();
                absenceCertificate = new StaffAbsenceCertificateDialog();

                //Assert
                Assert.AreEqual(selfSignatoryType, absenceCertificate.SignatoryType);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.DateRecieved);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.DateSigned);
                Assert.AreEqual(signedBy, absenceCertificate.SignedBy);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAbsenceCertificate", "P1", "Read", "Self")]
        public void Read_doctor_absence_certification_from_absence_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId), GetDoctorAbsenceCertificate(saID)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                var absence = new StaffAbsenceDialog();
                var acGridRow = absence.AbsenceCertificates.Rows[0];
                acGridRow.ClickEdit();
                var absenceCertificate = new StaffAbsenceCertificateDialog();

                //Assert
                Assert.AreEqual(doctorSignatoryType, absenceCertificate.SignatoryType);
                Assert.AreEqual("2", absenceCertificate.Duration);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.DateRecieved);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.DateSigned);
                Assert.AreEqual(signedBy, absenceCertificate.SignedBy);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.StartDate);
                Assert.AreEqual(endDate.ToShortDateString(), absenceCertificate.EndDate);
                Assert.AreEqual(nffw, absenceCertificate.CertificateAdvice);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAbsenceCertificate", "P1", "Read", "Doctor")]
        public void Read_self_absence_certification_from_absence_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId), GetSelfAbsenceCertificate(saID)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                var absence = new StaffAbsenceDialog();
                var acGridRow = absence.AbsenceCertificates.Rows[0];
                acGridRow.ClickEdit();
                var absenceCertificate = new StaffAbsenceCertificateDialog();

                //Assert
                Assert.AreEqual(selfSignatoryType, absenceCertificate.SignatoryType);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.DateRecieved);
                Assert.AreEqual(startDate.ToShortDateString(), absenceCertificate.DateSigned);
                Assert.AreEqual(signedBy, absenceCertificate.SignedBy);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAbsenceCertificate", "P1", "Update")]
        public void Update_self_absence_certification_in_absence_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            string newStartDate = startDate.AddDays(4).ToShortDateString();
            const string newSignedBy = "Mrs Test";

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId), GetSelfAbsenceCertificate(saID)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                var absence = new StaffAbsenceDialog();
                var acGridRow = absence.AbsenceCertificates.Rows[0];
                acGridRow.ClickEdit();

                var absenceCertificate = new StaffAbsenceCertificateDialog
                {
                    DateRecieved = newStartDate,
                    DateSigned = newStartDate,
                    SignedBy = newSignedBy
                };

                absenceCertificate.ClickOk();
                absence.ClickOk();
                staff.SaveStaff();
                staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                absence = new StaffAbsenceDialog();
                acGridRow = absence.AbsenceCertificates.Rows[0];
                acGridRow.ClickEdit();
                absenceCertificate = new StaffAbsenceCertificateDialog();

                //Assert
                Assert.AreEqual(selfSignatoryType, absenceCertificate.SignatoryType);
                Assert.AreEqual(newStartDate, absenceCertificate.DateRecieved);
                Assert.AreEqual(newStartDate, absenceCertificate.DateSigned);
                Assert.AreEqual(newSignedBy, absenceCertificate.SignedBy);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAbsenceCertificate", "P1", "Update")]
        public void Update_doctor_absence_certification_in_absence_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            string newStartDate = startDate.AddDays(4).ToShortDateString();
            string newendDate = startDate.AddDays(6).ToShortDateString();
            const string newSignedBy = "Mrs Test";

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId), GetDoctorAbsenceCertificate(saID)))
            {
                //Act
                LoginAndNavigate();
                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                var absence = new StaffAbsenceDialog();
                var acGridRow = absence.AbsenceCertificates.Rows[0];
                acGridRow.ClickEdit();

                var absenceCertificate = new StaffAbsenceCertificateDialog
                {
                    DateRecieved = newStartDate,
                    DateSigned = newStartDate,
                    SignedBy = newSignedBy,
                    StartDate = newStartDate,
                    EndDate = newendDate,
                    CertificateAdvice = certificateAdvice,
                    PhasedReturn = true,
                    AmendedDuties = true,
                    AlteredHours = true,
                    WorkplaceAdaptations = true,
                    IsReAssessmentRequired = true
                };

                absenceCertificate.ClickOk();
                absence.ClickOk();
                staff.SaveStaff();
                staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                absence = new StaffAbsenceDialog();
                acGridRow = absence.AbsenceCertificates.Rows[0];
                acGridRow.ClickEdit();
                absenceCertificate = new StaffAbsenceCertificateDialog();

                //Assert
                Assert.AreEqual(doctorSignatoryType, absenceCertificate.SignatoryType);
                Assert.AreEqual("3", absenceCertificate.Duration);
                Assert.AreEqual(newStartDate, absenceCertificate.DateRecieved);
                Assert.AreEqual(newStartDate, absenceCertificate.DateSigned);
                Assert.AreEqual(newSignedBy, absenceCertificate.SignedBy);
                Assert.AreEqual(newStartDate, absenceCertificate.StartDate);
                Assert.AreEqual(newendDate, absenceCertificate.EndDate);
                Assert.AreEqual(certificateAdvice, absenceCertificate.CertificateAdvice);
                Assert.AreEqual(true, absenceCertificate.PhasedReturn);
                Assert.AreEqual(true, absenceCertificate.AmendedDuties);
                Assert.AreEqual(true, absenceCertificate.AlteredHours);
                Assert.AreEqual(true, absenceCertificate.WorkplaceAdaptations);
                Assert.AreEqual(true, absenceCertificate.IsReAssessmentRequired);
            }
        }

        [TestMethod]
        [ChromeUiTest("StaffAbsenceCertificate", "P1", "Delete")]
        public void Delete_absence_certification_from_absence_as_PO()
        {
            //Arrange
            Guid staffId = Guid.NewGuid();
            Guid saID = Guid.NewGuid();
            Guid pacId = Guid.NewGuid();

            string forename = CoreQueries.GetColumnUniqueString("Staff", "LegalForename", 10, tenantID);
            string surname = CoreQueries.GetColumnUniqueString("Staff", "LegalSurname", 10, tenantID);
            string pacCode = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Code", 10, tenantID);
            string pacDescription = CoreQueries.GetColumnUniqueString("PayrollAbsenceCategory", "Description", 10, tenantID);

            using (new DataSetup(GetStaffRecord(staffId, forename, surname), GetPayrollAbsenceCategory(pacId, pacCode, pacDescription), GetStaffAbsence(saID, staffId, pacId), GetSelfAbsenceCertificate(saID)))
            {
                //Act
                LoginAndNavigate();

                StaffRecordPage staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                var gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                var absence = new StaffAbsenceDialog();
                var acGridRow = absence.AbsenceCertificates.Rows[0];
                acGridRow.DeleteRow();
                absence.ClickOk();
                staff.SaveStaff();

                staff = LoadStaff(staffId);
                staff.SelectAbsencesTab();
                gridRow = staff.Absences.Rows[0];
                gridRow.ClickEdit();
                absence = new StaffAbsenceDialog();
                acGridRow = absence.AbsenceCertificates.Rows.FirstOrDefault();

                //Assert
                Assert.AreEqual(null, acGridRow);
            }
        }

        #endregion

        #region Helpers

        private static void LoginAndNavigate()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.PersonnelOfficer);
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
        }

        private static StaffRecordPage LoadStaff(Guid staffId)
        {
            return StaffRecordPage.LoadStaffDetail(staffId);
        }

        #endregion

        #region Data Setup

        private DataPackage GetStaffRecord(Guid staffId, string forename, string surname)
        {
            return this.BuildDataPackage()
               .AddData("Staff", new
               {
                   Id = staffId,
                   LegalForename = forename,
                   LegalSurname = surname,
                   LegalMiddleNames = "Middle",
                   PreferredForename = forename,
                   PreferredSurname = surname,
                   DateOfBirth = new DateTime(2000, 1, 1),
                   Gender = CoreQueries.GetLookupItem("Gender", description: "Female"),
                   PolicyACLID = CoreQueries.GetPolicyAclId("Staff"),
                   School = CoreQueries.GetSchoolId(),
                   TenantID = tenantID
               })
               .AddData("StaffServiceRecord", new
               {
                   Id = Guid.NewGuid(),
                   DOA = startDate,
                   ContinuousServiceStartDate = startDate,
                   LocalAuthorityStartDate = startDate,
                   Staff = staffId,
                   TenantID = tenantID
               });
        }

        private DataPackage GetStaffAbsence(Guid saID, Guid staffId, Guid pacId)
        {
            return this.BuildDataPackage()
                .AddData("StaffAbsence", new
                {
                    Id = saID,
                    StartDate = startDate,
                    EndDate = endDate,
                    ExpectedReturnDate = endDate,
                    ActualReturnDate = endDate,
                    AbsencePayRate = CoreQueries.GetLookupItem("AbsencePayRate", tenantID, description: apr),
                    AbsenceType = CoreQueries.GetLookupItem("AbsenceType", tenantID, description: at),
                    IllnessCategory = CoreQueries.GetLookupItem("IllnessCategory", tenantID, description: ic),
                    PayrollAbsenceCategory = pacId,
                    WorkingDaysLost = workingDaysLost,
                    WorkingHoursLost = workingHoursLost,
                    AnnualLeave = true,
                    IndustrialInjury = true,
                    SSPExclusion = true,
                    Notes = notes,
                    Staff = staffId,
                    TenantID = tenantID
                });
        }

        private DataPackage GetPayrollAbsenceCategory(Guid pacId, string code, string description)
        {
            return this.BuildDataPackage()
                .AddData("PayrollAbsenceCategory", new
                {
                    ID = pacId,
                    Code = code,
                    Description = description,
                    IsVisible = "true",
                    DisplayOrder = "1",
                    TenantID = tenantID,
                    ResourceProvider = CoreQueries.GetSchoolId()
                });
        }

        private DataPackage GetSelfAbsenceCertificate(Guid abID)
        {
            return this.BuildDataPackage()
                .AddData("StaffAbsenceCertificate", new
                {
                    StaffAbsence = abID,
                    SignatoryType = CoreQueries.GetLookupItem("AbsenceCertificateSignatoryType", tenantID, description: selfSignatoryType),
                    DateReceived = startDate,
                    DateSigned = startDate,
                    SignedBy = signedBy,
                    TenantID = tenantID,
                });
        }

        private DataPackage GetDoctorAbsenceCertificate(Guid abID)
        {
            return this.BuildDataPackage()
                .AddData("StaffAbsenceCertificate", new
                {
                    StaffAbsence = abID,
                    SignatoryType = CoreQueries.GetLookupItem("AbsenceCertificateSignatoryType", tenantID, description: doctorSignatoryType),
                    DateReceived = startDate,
                    DateSigned = startDate,
                    SignedBy = signedBy,
                    StartDate = startDate,
                    EndDate = endDate,
                    CertificateAdvice = CoreQueries.GetLookupItem("AbsenceCertificateAdvice", tenantID, description: nffw),
                    TenantID = tenantID,
                });
        }

        #endregion
    }
}
