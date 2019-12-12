using System;

namespace ToolBox.Core.Domain
{
    /// <summary>
    /// An entity can implement this interface if <see cref="CreationTime"/> of this entity must be stored.
    /// <see cref="CreationTime"/> is automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface IHasCreationTime
    {
        /// <summary>
        /// 实体的创建时间
        /// </summary>
        DateTime CreationTime { get; set; }
    }
}