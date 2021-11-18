using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServicesLayer.ViewModel
{
    public class ClassEditServicesModel
    {
        public Guid ClassId { get; set; }
        public string Name { get; set; }
        public Guid[] StudentId { get; set; }
        public Guid[] SubjectId { get; set; }
        public Guid OldPresidentId { get; set; }
        public Guid OldSecretaryId { get; set; }
        public Guid NewPresidentId { get; set; }
        public Guid NewSecretaryId { get; set; }
    }
}
