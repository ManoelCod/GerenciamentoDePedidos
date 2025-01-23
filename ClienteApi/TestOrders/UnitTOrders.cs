using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiTask.Controllers;
using ApiTask.DTOs;
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
        private readonly Mock<PedidoApplication> _mockPedidoApplication;
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockPedidoApplication = new Mock<PedidoApplication>(MockBehavior.Strict);
            _mockOrderService = new Mock<IOrderService>(MockBehavior.Strict);
            _controller = new OrderController(_mockPedidoApplication.Object, _mockOrderService.Object);
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

            var pedido = new Pedido(Guid.NewGuid(), orderDto.name, DateTime.UtcNow);

            _mockPedidoApplication.Setup(service => service.CriarPedidoAsync(It.IsAny<Pedido>())).Returns(Task.FromResult(pedido));

            // Act
            var result = await _controller.CreatePedidoAsync(orderDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Pedido criado com sucesso!", ((dynamic)okResult.Value).mensagem);

            _mockPedidoApplication.Verify(service => service.CriarPedidoAsync(It.IsAny<Pedido>()), Times.Once);
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
                Id = "qTkPoMiVqZ4tG0UqYJr6",
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
            Assert.Equal("Pedido atualizado com sucesso!", ((dynamic)okResult.Value).mensagem);

            _mockPedidoApplication.Verify(service => service.ObterPedidoPorIdAsync(It.IsAny<Guid>()), Times.Once);
            _mockPedidoApplication.Verify(service => service.AtualizarPedidoAsync(It.IsAny<Pedido>()), Times.Once);
        }

        [Fact]
        public async Task UpdateOrderAsync_RetornaNotFound_QuandoPedidoNaoExiste()
        {
            // Arrange
            var orderDto = new OrderDTO
            {
                Id = "qTkPoMiVqZ4tG0UqYJr6",
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
            Assert.Equal("Pedido removido com sucesso!", ((dynamic)okResult.Value).mensagem);

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
            var okResult = Assert.IsType<OkObjectResult>(result);
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
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
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
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(pedidos, okResult.Value);

            _mockPedidoApplication.Verify(service => service.ObterTodosPedidosAsync(), Times.Once);
        }
    }
}
