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
        var doctor = new Doctor("D001", "Dr Mark", 0);
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

    /*
     * CO-PILOT SUGGESTED TESTS
     */

    // Sequential bookings should reduce slots once and not go negative
    [TestMethod]
    public void BookAppointment_SequentialBookings_SecondFailsAndSlotsNotNegative()
    {
        var doctor = new Doctor("D001", "Dr Mark", 1);
        var patient1 = new Patient("P001", "Alice");
        var patient2 = new Patient("P002", "Bob");
        var service = new AppointmentBookingService();

        var req1 = new AppointmentRequest(patient1, doctor, DateTime.Today.AddDays(1));
        var req2 = new AppointmentRequest(patient2, doctor, DateTime.Today.AddDays(1));

        BookingResult result1 = service.BookAppointment(req1);
        BookingResult result2 = service.BookAppointment(req2);

        Assert.IsTrue(result1.Success, "first booking should succeed");
        Assert.IsFalse(result2.Success, "second booking should fail when no slots left");
        Assert.AreEqual(0, doctor.AvailableSlots, "available slots must not go negative");
    }

    // Constructing an AppointmentRequest with null patient/doctor throws appropriate exceptions
    [TestMethod]
    public void AppointmentRequest_WhenPatientIsNull_ThrowsArgumentNullException()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        Assert.Throws<ArgumentNullException>(() => new AppointmentRequest(null, doctor, DateTime.Today));
    }
    [TestMethod]
    public void AppointmentRequest_WhenDoctorIsNull_ThrowsArgumentNullException()
    {
        var patient = new Patient("P001", "Alice");
        Assert.Throws<ArgumentNullException>(() => new AppointmentRequest(patient, null, DateTime.Today));
    }

    // Doctor.ReserveSlot enforces invariants by throwing when no slots available
    [TestMethod]
    public void Doctor_ReserveSlot_WhenNoSlots_ThrowsInvalidOperationException()
    {
        var doctor = new Doctor("D001", "Dr Mark", 0);
        Assert.Throws<InvalidOperationException>(() => doctor.ReserveSlot());
    }
}
