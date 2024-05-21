using AutoMapper;
using LEARN.data;
using LEARN.db.unitofwork;
using LEARN.model;
using MassTransit;
using Mediator.Net.Context;
using message;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.AccessControl;
using System.Threading;

namespace LEARN.rabbitmq
{
    public class createStaffConsumer : IConsumer<sendedevent>
    {
        private readonly IStaffrepository _staffrepository;
        private readonly iunitofwork _unitofwork;
        public createStaffConsumer(IStaffrepository staffrepository, iunitofwork unitofwork)
        {
            _staffrepository = staffrepository;
            _unitofwork = unitofwork;
        }

        public async Task Consume(ConsumeContext<sendedevent> context)
        {
            var position = new Staff
            {
                id = Guid.NewGuid(),
                name=context.Message.name,
                major= context.Message.major,
            };

            await Task.Delay(TimeSpan.FromSeconds(3));
            await _staffrepository.InsertAsync(position).ConfigureAwait(false);
            await _unitofwork.commitasync();
            Console.WriteLine("添加消费成功");

        }
    }
}
