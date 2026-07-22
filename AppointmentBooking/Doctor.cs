using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentBooking
{
    public class Doctor
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public int AvailableSlots { get; private set; }

        public Doctor(string id, string fullName, int availableSlots)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Doctor ID is required.");
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Doctor name is required.");
            }

            if(availableSlots < 0)
            {
                throw new ArgumentException("Available slots cannot be negative");
            }

            Id = id;
            FullName = fullName;
            AvailableSlots = availableSlots;
        }

        // Returns true if doctor has an available slot
        public bool HasAvailableSlot()
        {
            return AvailableSlots > 0;
        }

        // Decreases available slots by 1 if doctor has an available slot
        public void ReserveSlot()
        {
            if (!HasAvailableSlot())
            {
                throw new InvalidOperationException("No appointment slots are available");
            }

            AvailableSlots--;
        }
    }
}

/*

Detailed analysis of improvements:

Reliability: prevents invalid slot counts. - The improved version prevents invalid slot counts by checking that the inputted available slot count is > 0

Maintainability: keeps slot rules inside Doctor. - 

Encapsulaiton: prevents uncontrolled external changes. - Does this by using a private set for AvailableSlots, this way only the Doctor class can set the available slot count.

Testablility: makes slot behavior easier to test. - Does this by already checking some things and throwing errors with specific error messages, and adds new methods that we
can use for our testing.

 */