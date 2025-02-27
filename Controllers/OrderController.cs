using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
     [Route("pedido")]
     [ApiController]
     public class OrderController : ControllerBase
    {      
        /*Pedidos*/
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Order>>> Get(
            [FromServices]DataContext context
        )
        {
            var orders = await context.pedido            
            .AsNoTracking()
            .ToListAsync();
            return Ok(orders);
        } 

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Order>>> GetById(
            [FromServices]DataContext context,
            int id
        )
        {
            var orders = await context.pedido
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.id == id);
            return Ok(orders);
        }  

        [HttpGet]
        [Route("{id:int}/item")]
        public async Task<ActionResult<List<OrderItem>>> GetItemById(
            [FromServices]DataContext context,
            int id
        )
        {
            var order = await context.pedido
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.id == id);

            var orderItem = await context.pedido_item
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.idPedido == id);

            var orderItemInList = new List<OrderItem>();
            orderItemInList.Add(orderItem);

            var OrderItemList = new OrderItemList(
                order.id, order.idVendedor, order.idPessoa, orderItemInList);

            return Ok(OrderItemList);
        }              
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<List<Order>>> Post(
            [FromBody]Order model,
            [FromServices]DataContext context
            )
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
        try{
            context.pedido.Add(model);  
            await context.SaveChangesAsync();
            return Ok(model);        
            }
        catch{
            return BadRequest(new  { message = "Não foi possível criar o pedido!"});
            } 
        }
        
    }
}