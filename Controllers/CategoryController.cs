using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimApi.Base;
using SimApi.Data;
using SimApi.Data.Context;
using SimApi.Data.Repository;
using SimApi.Schema;

namespace SimApi.Service.Controllers;

[Route("simapi/v1/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private SimDbContext _context;
    private ICategoryRepository repository;
    private IMapper mapper;
    public CategoryController(SimDbContext context, ICategoryRepository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
        _context = context;
    }


    [HttpGet]
    public ApiResponse<List<CategoryResponse>> GetAll()
    {
        var list = _context.Set<Category>().Where(x => x.IsValid).Include(x => x.Products).ToList();
        var mapped = mapper.Map<List<Category>, List<CategoryResponse>>(list);
        return new ApiResponse<List<CategoryResponse>>(mapped);
    }

    [HttpGet("{id}")]
    public ApiResponse<CategoryResponse> GetById(int id)
    {
        var row = _context.Set<Category>().Where(x => x.Id == id ).Include(x => x.Products).FirstOrDefault();
        var mapped = mapper.Map<Category, CategoryResponse>(row);
        return new ApiResponse<CategoryResponse>(mapped);
    }

    [HttpGet("Count")]
    public int Count()
    {
        var row = repository.GetAllCount();
        return row;
    }

    [HttpPost]
    public ApiResponse Post([FromBody] CategoryRequest request)
    {
        var mapped = mapper.Map<CategoryRequest, Category>(request);
        var entity = _context.Set<Category>().Add(mapped);
        _context.SaveChanges();
        return new ApiResponse();
    }

    [HttpPut("{id}")]
    public ApiResponse Put(int id, [FromBody] CategoryRequest request)
    {
        var mapped = mapper.Map<CategoryRequest, Category>(request);
        mapped.Id = id;
        var entity = _context.Set<Category>().Update(mapped);
        _context.SaveChanges();
        return new ApiResponse();
      
    }


    [HttpDelete("{id}")]
    public ApiResponse Delete(int id)
    {
        var entity = _context.Set<Category>().Find(id);
        _context.Set<Category>().Remove(entity);
        _context.SaveChanges();
        return new ApiResponse();
    }

}
