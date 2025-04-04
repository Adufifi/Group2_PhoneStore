
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
            var statusResponse = new StatusResponse();
            if (!ModelState.IsValid)
            {
                statusResponse.status = -999;
                statusResponse.mess = "Input invalid";
                return BadRequest(statusResponse);
            }

            var review = _mapper.Map<Review>(reviewDto);
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
        [HttpGet("getReviewsByProductId/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(Guid productId)
        {
            var reviews = await _reviewServices.GetAllAsync();
            var filteredReviews = reviews.Where(review => review.ProductId == productId).ToList();

            if (filteredReviews.Any())
            {
                return Ok(filteredReviews);
            }

            return NotFound("Không tìm thấy reviews cho productId: " + productId);
        }

        [HttpDelete("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(Guid id, [FromQuery] Guid accountId, [FromQuery] Guid productId)
        {
            var statusResponse = new StatusResponse();
            var reviewToDelete = await _reviewServices.GetByIdAsync(id);
            if (reviewToDelete == null)
            {
                statusResponse.status = -1;
                statusResponse.mess = "Review not found";
                return NotFound(statusResponse);
            }
            if (reviewToDelete.AccountId != accountId || reviewToDelete.ProductId != productId)
            {
                statusResponse.status = -999;
                statusResponse.mess = "AccountId and ProductId do not match the review";
                return BadRequest(statusResponse);
            }

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
