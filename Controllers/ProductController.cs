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
public class ProductController : ControllerBase
{
    private SimDbContext _context;
    private readonly IProductRepository repository;
    private IMapper mapper;
    public ProductController(SimDbContext context, IMapper mapper, IProductRepository repository)
    {
        this.repository = repository;
        this.mapper = mapper;
        _context = context;


    }

    [HttpGet]
    public ApiResponse<List<ProductResponse>> GetAll()
    {
        var list = _context.Set<Product>().Where(x => x.IsValid).ToList();
        var mapped = mapper.Map<List<ProductResponse>>(list);
        return new ApiResponse<List<ProductResponse>>(mapped);
    }

    [HttpGet("{id}")]
    public ApiResponse<ProductResponse> GetById(int id)
    {
        var row = _context.Set<Product>().Where(p => p.Id == id).FirstOrDefault();

        if (row == null)
        {
            return new ApiResponse<ProductResponse>("Product not found");
        }

        var mapped = mapper.Map<ProductResponse>(row);
        return new ApiResponse<ProductResponse>(mapped);
    }


    [HttpPost]
    public ApiResponse Post([FromBody] ProductRequest request)
    {
        var mapped = mapper.Map<ProductRequest, Product>(request);
        var entity = _context.Set<Product>().Add(mapped);
        _context.SaveChanges();
        return new ApiResponse();
    }

    [HttpPut("{id}")]
    public ApiResponse Put(int id, [FromBody] ProductRequest request)
    {
        var mapped = mapper.Map<ProductRequest, Product>(request);
        mapped.Id = id;
        var entity = _context.Set<Product>().Add(mapped);
        _context.SaveChanges();
        return new ApiResponse();
    }


    [HttpDelete("{id}")]
    public ApiResponse Delete(int id)
    {
        var entity = _context.Set<Product>().Find(id);
        _context.Set<Product>().Remove(entity);
        _context.SaveChanges();
        return new ApiResponse();
    }

}
