using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace IDVDriver.Utils
{
    [Serializable]
    public class BusinessException : Exception
    {
        public IList<ValidationFailure> Errors { get; set; } = new List<ValidationFailure>();

        public BusinessException()
        {

        }

        public BusinessException(string message, IList<ValidationFailure> errors)
            : base(message)
        {
            Errors = errors;
        }
    }
}