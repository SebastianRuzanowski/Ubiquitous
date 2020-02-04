﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace U.SmartStoreAdapter.Domain.Entities.Catalog
{
	/// <summary>
	/// Represents a specification attribute option
	/// </summary>
	[DataContract]
	public class SpecificationAttributeOption : BaseEntity
	{
        private ICollection<ProductSpecificationAttribute> _productSpecificationAttributes;

        /// <summary>
        /// Gets or sets the specification attribute identifier
        /// </summary>
		[DataMember]
		public int SpecificationAttributeId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the specification attribute option alias
		/// </summary>
		[DataMember]
		public string Alias { get; set; }

		/// <summary>
		/// Gets or sets the display order
		/// </summary>
		[DataMember]
		public int DisplayOrder { get; set; }

		/// <summary>
		/// Gets or sets the number value for range filtering
		/// </summary>
		[DataMember]
		public decimal NumberValue { get; set; }

		/// <summary>
		/// Gets or sets the specification attribute
		/// </summary>
		[DataMember]
		public virtual SpecificationAttribute SpecificationAttribute { get; set; }

        /// <summary>
        /// Gets or sets the product specification attributes
        /// </summary>
		[DataMember]
		public virtual ICollection<ProductSpecificationAttribute> ProductSpecificationAttributes
        {
			get => _productSpecificationAttributes ?? (_productSpecificationAttributes = new HashSet<ProductSpecificationAttribute>());
			protected set => _productSpecificationAttributes = value;
        }
    }
}