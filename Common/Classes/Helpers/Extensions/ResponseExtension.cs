
using App.Common.Classes.DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Common.Classes.Extensions
{
   public static class ResponseExtension
    {
        public static ResponseDTO<T> AsResponseDTO<T>(this T resultDTO, int code, string message = "")
        {
            var responseDTO = new ResponseDTO<T>();
            responseDTO.Data = resultDTO;
            responseDTO.Header = new HeaderDTO() { ReponseCode = code, Message = message };

            return responseDTO;
        }
    }
}
