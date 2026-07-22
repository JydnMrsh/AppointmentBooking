using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentBooking
{
    public class AppointmentBookingService
    {
        public bool BookAppointment(AppointmentRequest request)
        {
            if (request.Doctor.AvailableSlots <= 0)
            {
                return false; // No available slots
            }

            request.Doctor.AvailableSlots--;
            return true; // Appointment booked successfully
        }
    }
}
