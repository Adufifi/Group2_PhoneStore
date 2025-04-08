
namespace PhoneStore.Api.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewServices _reviewServices;
        private readonly IMapper _mapper;

        public ReviewController(IReviewServices reviewServices, IMapper mapper)
        {
            _reviewServices = reviewServices;
            _mapper = mapper;
        }
        [HttpGet("All")]
        public async Task<IActionResult> All()
        {
            var data = await _reviewServices.GetAllAsync();
            return Ok(data);
        }
        [HttpGet("GetReviewById/{id}")]
        public async Task<IActionResult> GetReviewById(Guid id)
        {
            var data = await _reviewServices.GetByIdAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        [HttpPost("CreateReview")]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewDto)
        {
            try
            {
                var statusResponse = new StatusResponse();
                if (!ModelState.IsValid)
                {
                    statusResponse.status = -999;
                    statusResponse.mess = "Input invalid";
                    return BadRequest(statusResponse);
                }

                var review = new Review();
                review.Id = Guid.NewGuid();
                review.AccountId = reviewDto.AccountId;
                review.ProductId = reviewDto.ProductId;
                review.Comment = reviewDto.Comment;
                review.CreatedDate = DateTime.Now;
                if (review.ProductId == Guid.Empty || review.AccountId == Guid.Empty)
                {
                    statusResponse.status = -999;
                    statusResponse.mess = "ProductId or AccountId cannot be empty";
                    return BadRequest(statusResponse);
                }

                var result = await _reviewServices.AddAsync(review);
                if (result > 0)
                {
                    statusResponse.status = 1;
                    statusResponse.mess = "Add success";
                    return Ok(statusResponse);
                }

                statusResponse.status = -2;
                statusResponse.mess = "Add failed";
                return StatusCode(500, statusResponse);
            }
            catch (Exception ex)
            {

                var statusResponse = new StatusResponse();
                statusResponse.status = -2;
                statusResponse.mess = ex.Message;
                return Ok(statusResponse);
            }
        }
        [HttpGet("getReviewsByProductId/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(Guid productId)
        {
            var data = await _reviewServices.GetAllAsync();
            var dataReturn = data.Where(x => x.ProductId == productId).ToList();
            if (dataReturn == null)
            {
                return NotFound();
            }
            return Ok(dataReturn);
        }
        [HttpPut("UpdateReview/{id}")]
        public async Task<IActionResult> UpdateReview(Guid id, [FromBody] ReviewDto reviewDto)
        {
            var statusResponse = new StatusResponse();
            var reviewToUpdate = await _reviewServices.GetByIdAsync(id);
            if (reviewToUpdate == null)
            {
                statusResponse.status = -1;
                statusResponse.mess = "Review not found";
                return NotFound(statusResponse);
            }

            if (!ModelState.IsValid)
            {
                statusResponse.status = -909;
                statusResponse.mess = "Input invalid";
                return BadRequest(statusResponse);
            }
            if (reviewToUpdate.AccountId != reviewDto.AccountId || reviewToUpdate.ProductId != reviewDto.ProductId)
            {
                statusResponse.status = -999;
                statusResponse.mess = "AccountId and ProductId do not match the review";
                return BadRequest(statusResponse);
            }

            _mapper.Map(reviewDto, reviewToUpdate);

            var result = await _reviewServices.UpdateAsync(reviewToUpdate);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Update success";
                return Ok(statusResponse);
            }

            statusResponse.status = -2;
            statusResponse.mess = "Update failed";
            return StatusCode(500, statusResponse);
        }
        [HttpDelete("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var statusResponse = new StatusResponse();

            var result = await _reviewServices.DeleteAsync(id);
            if (result)
            {
                statusResponse.status = 1;
                statusResponse.mess = "Delete success";
                return Ok(statusResponse);
            }

            statusResponse.status = -2;
            statusResponse.mess = "Delete failed";
            return StatusCode(500, statusResponse);
        }
    }
}
