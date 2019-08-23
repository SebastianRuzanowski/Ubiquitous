using System;

namespace U.ProductService.Application.Exceptions
{
    [Serializable]
    public class PictureNotFoundException : ProductServiceApplicationBaseException
    {
        public PictureNotFoundException(string message)
            : base(message)
        {
        }

        public PictureNotFoundException(string message, Exception inner): base(message, inner)
        {
        }   
    }
}