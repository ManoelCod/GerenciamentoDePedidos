using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ApiTask.Controllers;
using ApiTask.DTOs;
using ApiTask.MediatR;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManagement.Application;
using TaskManagement.Application.Service.Interface;
using TaskManagement.Domain.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace ApiTask.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<IPedidoService> _mockPedidoService;
        private readonly Mock<PedidoApplication> _mockPedidoApplication;
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockPedidoService = new Mock<IPedidoService>(MockBehavior.Strict);
            _mockPedidoApplication = new Mock<PedidoApplication>(_mockPedidoService.Object); // Removido MockBehavior.Strict
            _mockOrderService = new Mock<IOrderService>(MockBehavior.Strict);
            _mockMediator = new Mock<IMediator>(MockBehavior.Strict);
            _controller = new OrderController(_mockPedidoApplication.Object, _mockOrderService.Object, _mockMediator.Object);
        }

        [Fact]
        public async Task CreatePedidoAsync_RetornaOkResult_QuandoPedidoEhValido()
        {
            // Arrange
            var orderDto = new OrderDTO
            {
                name = "Poliana",
                Carts = "[{\"Name\":\"Hambúrguer clássico\",\"Quantity\":1,\"Price\":10.50},{\"Name\":\"Cheeseburger\",\"Quantity\":1,\"Price\":12.50}]"
            };

            var pedidoId = Guid.NewGuid();

            _mockMediator
                .Setup(m => m.Send(It.IsAny<CriarPedidoCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pedidoId);

            // Act
            var result = await _controller.CreatePedidoAsync(orderDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Converter o valor da resposta para JsonElement
            var responseJson = JsonSerializer.Serialize(okResult.Value);
            var responseElement = JsonSerializer.Deserialize<JsonElement>(responseJson);

            // Acessar a propriedade mensagem
            var mensagem = responseElement.GetProperty("mensagem").GetString();
            Assert.Equal("Pedido criado com sucesso!", mensagem);

            _mockMediator.Verify(m => m.Send(It.IsAny<CriarPedidoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePedidoAsync_RetornaBadRequest_QuandoPedidoEhNulo()
        {
            // Act
            var result = await _controller.CreatePedidoAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Pedido não pode ser nulo.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateOrderAsync_RetornaOkResult_QuandoPedidoEhValido()
        {
            // Arrange
            var orderDto = new OrderDTO
            {
                Id = Guid.NewGuid().ToString(), 
                name = "Poliana",
                Carts = "[{\"Name\":\"Hambúrguer clássico\",\"Quantity\":1,\"Price\":10.50},{\"Name\":\"Cheeseburger\",\"Quantity\":1,\"Price\":12.50}]"
            };
            var pedidoExistente = new Pedido(Guid.Parse(orderDto.Id), orderDto.name, DateTime.UtcNow);

            _mockPedidoApplication.Setup(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(pedidoExistente);
            _mockPedidoApplication.Setup(service => service.AtualizarPedidoAsync(It.IsAny<Pedido>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateOrderAsync(Guid.Parse(orderDto.Id), orderDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            // Converter o valor da resposta para JsonElement
            var responseJson = JsonSerializer.Serialize(okResult.Value);
            var responseElement = JsonSerializer.Deserialize<JsonElement>(responseJson);

            // Acessar a propriedade mensagem
            var mensagem = responseElement.GetProperty("mensagem").GetString();
            Assert.Equal("Pedido atualizado com sucesso!", mensagem);

            _mockPedidoApplication.Verify(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>()), Times.Once);
            _mockPedidoApplication.Verify(service => service.AtualizarPedidoAsync(It.IsAny<Pedido>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_RetornaNotFound_QuandoPedidoNaoExiste()
        {
            // Arrange
            var orderDto = new OrderDTO
            {
                Id = Guid.NewGuid().ToString(),
                name = "Poliana",
                Carts = "[{\"Name\":\"Hambúrguer clássico\",\"Quantity\":1,\"Price\":10.50},{\"Name\":\"Cheeseburger\",\"Quantity\":1,\"Price\":12.50}]"
            };

            _mockPedidoApplication.Setup(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Pedido)null);

            // Act
            var result = await _controller.UpdateOrderAsync(Guid.Parse(orderDto.Id), orderDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Pedido não encontrado.", notFoundResult.Value);

            _mockPedidoApplication.Verify(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_RetornaBadRequest_QuandoPedidoEhNulo()
        {
            // Act
            var result = await _controller.UpdateOrderAsync(Guid.NewGuid(), null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("Pedido não pode ser nulo e o ID deve coincidir.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteOrderAsync_RetornaOkResult_QuandoPedidoExiste()
        {
            // Arrange
            var pedidoExistente = new Pedido(Guid.NewGuid(), "Poliana", DateTime.UtcNow);

            _mockPedidoApplication.Setup(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(pedidoExistente);
            _mockPedidoApplication.Setup(service => service.RemoverPedidoAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteOrderAsync(pedidoExistente.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            // Converter o valor da resposta para JsonElement
            var responseJson = JsonSerializer.Serialize(okResult.Value);
            var responseElement = JsonSerializer.Deserialize<JsonElement>(responseJson);

            // Acessar a propriedade mensagem
            var mensagem = responseElement.GetProperty("mensagem").GetString();
            Assert.Equal("Pedido removido com sucesso!", mensagem);

            _mockPedidoApplication.Verify(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>()), Times.Once);
            _mockPedidoApplication.Verify(service => service.RemoverPedidoAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_RetornaNotFound_QuandoPedidoNaoExiste()
        {
            // Arrange
            _mockPedidoApplication.Setup(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Pedido)null);

            // Act
            var result = await _controller.DeleteOrderAsync(Guid.NewGuid());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Pedido não encontrado.", notFoundResult.Value);

            _mockPedidoApplication.Verify(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetOrderByIdAsync_RetornaOkResult_QuandoPedidoExiste()
        {
            // Arrange
            var pedidoExistente = new Pedido(Guid.NewGuid(), "Poliana", DateTime.UtcNow);

            _mockPedidoApplication.Setup(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(pedidoExistente);

            // Act
            var result = await _controller.GetOrderByIdAsync(pedidoExistente.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(pedidoExistente, okResult.Value);

            _mockPedidoApplication.Verify(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>()), Times.Once);
        }


        [Fact]
        public async Task GetOrderByIdAsync_RetornaNotFound_QuandoPedidoNaoExiste()
        {
            // Arrange
            _mockPedidoApplication.Setup(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Pedido)null);

            // Act
            var result = await _controller.GetOrderByIdAsync(Guid.NewGuid());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);

            _mockPedidoApplication.Verify(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task GetAllOrdersAsync_RetornaOkResult_QuandoPedidosExistem()
        {
            // Arrange
            var pedidos = new List<Pedido>
    {
        new Pedido(Guid.NewGuid(), "Poliana", DateTime.UtcNow),
        new Pedido(Guid.NewGuid(), "Outro Cliente", DateTime.UtcNow)
    };

            _mockPedidoApplication.Setup(service => service.ObterTodosPedidosAsync()).ReturnsAsync(pedidos);

            // Act
            var result = await _controller.GetAllOrdersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(pedidos, okResult.Value);

            _mockPedidoApplication.Verify(service => service.ObterTodosPedidosAsync(), Times.Once);
        }

        [Fact]
        public async Task CalcularValorTotalAsync_RetornaOkResult_QuandoPedidoIdEhValido()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var valorTotalEsperado = 123.45m; // Valor total esperado do pedido

            _mockPedidoApplication.Setup(service => service.CalcularValorTotalPedidoAsync(It.IsAny<Guid>())).ReturnsAsync(valorTotalEsperado);

            // Act
            var result = await _controller.CalcularValorTotalAsync(pedidoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(valorTotalEsperado, okResult.Value);

            _mockPedidoApplication.Verify(service => service.CalcularValorTotalPedidoAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
