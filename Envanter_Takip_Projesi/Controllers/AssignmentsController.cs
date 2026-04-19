using Application.DTOs.Assignments;
using Application.DTOs.Common;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Envanter_Takip_Projesi.Controllers
{
    public class AssignmentsController(IAssignmentService assignmentService) : CustomBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assignments = await assignmentService.GetAllAssignmentsAsync();
            return CreateActionResultInstance(Response<List<AssignmentDto>>.Success(assignments, 200));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var assignment = await assignmentService.GetByIdAsync(id);
            return CreateActionResultInstance(Response<AssignmentDto>.Success(assignment, 200));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAssignmentDto createAssignmentDto)
        {
            await assignmentService.CreateAsync(createAssignmentDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAssignmentDto updateAssignmentDto)
        {
            await assignmentService.UpdateAsync(updateAssignmentDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        // ÖZEL ENDPOINT: Zimmet İade Alma
        // İstek Adresi: api/Assignments/return
        [HttpPut("return")]
        public async Task<IActionResult> ReturnItem(ReturnAssignmentDto returnAssignmentDto)
        {
            await assignmentService.ReturnItemAsync(returnAssignmentDto);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await assignmentService.DeleteAsync(id);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}