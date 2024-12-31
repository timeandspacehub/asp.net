using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using asp.net.Dtos.Comment;
using asp.net.Models;

namespace asp.net.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment commentModel){
            return new CommentDto{
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            };
        }
    }
}