using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class PedidoController : BaseController
{
    private readonly IPedidoService _pedidoService;

    public PedidoController(
        IPedidoService pedidoService,
        ILogger<PedidoController> logger) : base(logger)
    {
        _pedidoService = pedidoService;
    }

    [HttpPost]
    public async Task<ActionResult<PedidoDTO>> Create(CreatePedidoDTO createPedidoDTO)
    {
        try
        {
            var pedido = await _pedidoService.CreatePedidoAsync(createPedidoDTO);
            return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, new PedidoDTO(pedido));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoDTO>> GetById(int id)
    {
        try
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            return Ok(new PedidoDTO(pedido));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}