using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServicesLayer.ViewModel
{
    public class ClassEditServicesModel
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public int[] StudentId { get; set; }
        public int[] SubjectId { get; set; }
        public int OldPresidentId { get; set; }
        public int OldSecretaryId { get; set; }
        public int NewPresidentId { get; set; }
        public int NewSecretaryId { get; set; }
    }
}
