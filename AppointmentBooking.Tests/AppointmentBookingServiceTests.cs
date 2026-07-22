using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppointmentBooking.Tests;

[TestClass]
public class AppointmentBookingServiceTests
{
    /*
     * BOOKING SERVICE TESTS
     */

    // Test method to check if booking an appointment when the doctor has available slots returns success
    [TestMethod]
    public void BookAppointment_WhenDoctorHasAvailableSlots_ReturnsSuccess()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();

        BookingResult result = service.BookAppointment(request);

        Assert.IsTrue(result.Success);
    }

    // Test method to check if booking an appointment when the doctor has no available slots returns failure
    [TestMethod]
    public void BookAppointment_WhenDoctorHasNoAvailableSlots_ReturnsFailure()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();

        BookingResult result = service.BookAppointment(request);

        Assert.IsFalse(result.Success);
    }

    // Test method to check if booking an appointment when the doctor has available slots decreases the available slots count
    [TestMethod]
    public void BookAppointment_WhenSuccessful_DecreasesAvailableSlots()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();

        service.BookAppointment(request);

        Assert.AreEqual(1, doctor.AvailableSlots);
    }

    // Test method to check if booking an appointment when the doctor has no available slots does not decrease the available slots count
    [TestMethod]
    public void BookAppointment_WhenFailed_DoesNotDecreaseAvailableSlots()
    {
        var doctor = new Doctor("D001", "Dr Mark", 0);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();

        service.BookAppointment(request);

        Assert.AreEqual(0, doctor.AvailableSlots);
    }

    // Test method to check if booking an appointment when successful returns a helpful message
    [TestMethod]
    public void BookAppointment_WhenSuccessful_ReturnsHelpfulMessage()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        var patient = new Patient("P001", "Diana William", "Aroha");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();
        BookingResult result = service.BookAppointment(request);
        StringAssert.Contains(result.Message, "Appointment booked successfully");
        StringAssert.Contains(result.Message, "Aroha");
    }

    // Test method to check if booking an appointment when there are no available slots returns a helpful message
    [TestMethod]
    public void BookAppointment_WhenNoSlots_ReturnsHelpfulMessage()
    {
        var doctor = new Doctor("D001", "Dr Mark", 0);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();
        BookingResult result = service.BookAppointment(request);
        StringAssert.Contains(result.Message, "no available slots");
    }

    /*
     * VALIDATION TESTS
     */

    // Test method to check if creating a doctor with an empty ID throws an exception
    [TestMethod]
    public void Doctor_WhenIdIsEmpty_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Doctor("", "Dr Mark", 2));
    }

    // Test method to check if creating a doctor with negative available slots throws an exception
    [TestMethod]
    public void Doctor_WhenAvailableSlotsIsNegative_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Doctor("D001", "Dr Mark", -1));
    }

    // Test method to check if creating a patient with an empty ID throws an exception
    [TestMethod]
    public void Patient_WhenIdIsEmpty_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Patient("", "Diana William"));
    }

    // Test method to check if creating a patient with a preferred name sets the display name to the preferred name
    [TestMethod]
    public void Patient_WhenPreferredNameExists_DisplayNameUsesPreferredName()
    {
        var patient = new Patient("P001", "Diana William", "Aroha");
        Assert.AreEqual("Aroha", patient.DisplayName);
    }

    // Test method to check if creating a patient without a preferred name sets the display name to the legal name
    [TestMethod]
    public void Patient_WhenPreferredNameMissing_DisplayNameUsesLegalName()
    {
        var patient = new Patient("P001", "Diana William");
        Assert.AreEqual("Diana William", patient.DisplayName);
    }

    // Test method to check if creating an appointment request with a requested date in the past throws an exception
    [TestMethod]
    public void AppointmentRequest_WhenRequestedDateIsInPast_ThrowsException()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        var patient = new Patient("P001", "Diana William");
        Assert.Throws<ArgumentException>(() =>
        new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(-1)));
    }
}
