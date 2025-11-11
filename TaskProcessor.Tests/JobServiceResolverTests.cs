using System;
using System.Collections.Generic;
using Application.Services;
using Moq;
using TaskProcessor.Application.Interfaces;
using TaskProcessor.Application.Services;
using TaskProcessor.Domain.interfaces;
using Xunit;

namespace TaskProcessor.Tests
{
    public class JobServiceResolverTests
    {
        [Fact]
        public void Deve_Retornar_Servico_Correto_Para_Tipo_Valido()
        {
            var emailServiceMock = new Mock<IJobService>();
            emailServiceMock.Setup(s => s.Tipo).Returns("EmailJob");

            var reportServiceMock = new Mock<IJobService>();
            reportServiceMock.Setup(s => s.Tipo).Returns("ReportJob");

            var services = new List<IJobService> { emailServiceMock.Object, reportServiceMock.Object };
            var resolver = new JobServiceResolver(services);

            var result = resolver.Resolver("ReportJob");

            Assert.Equal(reportServiceMock.Object, result);
        }

        [Fact]
        public void Deve_Lancar_Excecao_Para_Tipo_Invalido()
        {
            var emailServiceMock = new Mock<IJobService>();
            emailServiceMock.Setup(s => s.Tipo).Returns("EmailJob");

            var services = new List<IJobService> { emailServiceMock.Object };
            var resolver = new JobServiceResolver(services);

            Assert.Throws<InvalidOperationException>(() => resolver.Resolver("OutroJob"));
        }
    }
}