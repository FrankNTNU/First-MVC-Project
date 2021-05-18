using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class GeneralDTO
    {
        public List<PostDTO> SliderPosts { get; set; }
        public List<PostDTO> BreakingPosts { get; set; }
        public List<PostDTO> PopularPosts { get; set; }
        public List<PostDTO> MostViewedPosts { get; set; }
        public PostDTO PostDetail { get; set; }
        public List<VideoDTO> Videos { get; set; }
        public List<AdsDTO> AdsList { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string  Subject { get; set; }
        public string SearchText { get; set; }
        public int PostID { get; set; }
        public List<PostDTO> CategoryPostList { get; set; }
        public string CategoryName { get; set; }
        public AddressDTO Address { get; set; }
    }
}
