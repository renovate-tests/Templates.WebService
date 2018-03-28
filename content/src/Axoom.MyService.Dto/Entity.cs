using System.ComponentModel.DataAnnotations;

namespace Axoom.MyService.Dto
{
    /// <summary>
    /// A sample entity.
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// The ID of the entity.
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// The data of the entity.
        /// </summary>
        public string Data { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return (obj is Entity other)
                   && Id == other.Id
                   && Data == other.Data;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Id != null ? Id.GetHashCode() : 0) * 397) ^ (Data != null ? Data.GetHashCode() : 0);
            }
        }
    }
}