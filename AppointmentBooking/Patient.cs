using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentBooking
{
    public class Patient
    {
        public string Id { get; }
        public string LegalName { get; }
        public string PreferredName { get; }

        // Get method for display name, which returns the preferred name if available, otherwise returns the legal name
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(PreferredName))
                {
                    return LegalName;
                }

                return PreferredName;
            }
        }

        public Patient(string id, string legalName, string preferredName = "")
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Patient ID is required.");
            }

            if (string.IsNullOrWhiteSpace(legalName))
            {
                throw new ArgumentException("Legal name is required.");
            }

            Id = id;
            LegalName = legalName;
            PreferredName = preferredName;
        }
    }
}
