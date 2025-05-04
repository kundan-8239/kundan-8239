using System.ComponentModel.DataAnnotations;

namespace microservice_1.Models
{
    /// <summary>
    /// Request for creating a thing.
    /// </summary>
    /// 
    public class UpdateThingRequest
    {
        /// <summary>
        /// The new name for the thing
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
        public string NewName { get; set; }
    }
}
