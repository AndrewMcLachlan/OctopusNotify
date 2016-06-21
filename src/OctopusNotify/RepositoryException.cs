using System;
using System.Runtime.Serialization;

namespace OctopusNotify
{
    public class RepositoryException : ApplicationException
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class.
        /// </summary>
        public RepositoryException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified error message. 
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public RepositoryException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified. </param>
        public RepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected RepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the <see cref="SerializationInfo"/>  with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");

            base.GetObjectData(info, context);
        }
        #endregion
    }
}
