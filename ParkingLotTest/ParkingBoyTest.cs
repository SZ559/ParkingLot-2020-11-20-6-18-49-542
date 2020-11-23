﻿using ParkingLotCLI;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ParkingLotCLITest
{
    public class ParkingBoyTest
    {
        [Fact]
        public void Should_Parking_Boy_Park_A_Car_And_Get_Ticket()
        {
            //given
            var expectedTicket = new Ticket("N98245");

            //when
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });
            var ticket = parkingBoy.Park(new Car("N98245"), out _);

            //then
            Assert.Equal(expectedTicket.GetType(), ticket.GetType());
        }

        [Fact]
        public void Should_Parking_Boy_Fetch_A_Car_Using_Ticket()
        {
            //given
            var expectedCar = new Car("N98245");

            //when
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });
            var ticket = parkingBoy.Park(expectedCar, out _);
            var car = parkingBoy.Fetch(ticket, out _);

            //then
            Assert.Equal(expectedCar.GetType(), car.GetType());
        }

        [Fact]
        public void Should_Parking_Boy_Park_Mutiple_Cars_And_Fetch_The_Right_Car_Using_Ticket()
        {
            //given
            var expectedCar = new Car("N98245");

            //when
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });
            var carList = new List<Car>() { new Car("car1"), new Car("car2"), new Car("car3") };
            carList.ForEach(car => parkingBoy.Park(car, out _));
            var ticket = parkingBoy.Park(expectedCar, out _);
            var car = parkingBoy.Fetch(ticket, out _);

            //then
            Assert.Equal(expectedCar, car);
        }

        [Fact]
        public void Should_Parking_Boy_Fetch_No_Cars_Given_The_Wrong_Ticket()
        {
            //given
            var expectedCar = new Car("N98245");
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });
            parkingBoy.Park(expectedCar, out _);
            var wrongTicket = new Ticket("wrongNumber");

            //when
            var car = parkingBoy.Fetch(wrongTicket, out _);

            //then
            Assert.Null(car);
        }

        [Fact]
        public void Should_Parking_Boy_Fetch_No_Cars_Given_The_No_Ticket()
        {
            //given
            var expectedCar = new Car("N98245");

            //when
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });
            var car = parkingBoy.Fetch(null, out _);

            //then
            Assert.Null(car);
        }

        [Fact]
        public void Should_Parking_Boy_Fetch_No_Car_Given_The_Ticket_Has_Been_Used()
        {
            //given
            var parkedCar = new Car("N98245");
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });
            var ticket = parkingBoy.Park(parkedCar, out _);
            parkingBoy.Fetch(ticket, out _);

            //when
            var car = parkingBoy.Fetch(ticket, out _);

            //then
            Assert.Null(car);
        }

        [Fact]
        public void Should_Park_No_Car_Given_Parking_Lot_Has_No_Position()
        {
            //given
            var parkedCar = new Car("N98245");
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot(1) });
            parkingBoy.Park(parkedCar, out _);

            //when
            var newCar = new Car("A982453");
            var newTicket = parkingBoy.Park(newCar, out _);

            //then
            Assert.Null(newTicket);
        }

        [Fact]
        public void Should_The_Defualt_Capacity_Of_Parking_Lot_Is_10()
        {
            //when
            var parkingLot = new ParkingLot();

            //then
            Assert.Equal(10, parkingLot.Capacity);
        }

        [Fact]
        public void Should_Get_Error_Message_Unrecognized_parking_ticket_Given_A_Wrong_Ticket_When_Fetch_Car()
        {
            //given
            var guid = Guid.NewGuid();
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot(10) });
            var wrongTicket = new Ticket("wrongNumber");

            //when
            var errorMessage = string.Empty;
            parkingBoy.Fetch(wrongTicket, out errorMessage);

            //then
            Assert.Equal("Unrecognized parking ticket.", errorMessage);
        }

        [Fact]
        public void Should_Get_Error_Message_Unrecognized_parking_ticket_Given_A_Used_Ticket_When_Fetch_Car()
        {
            //given
            var parkedCar = new Car("N98245");
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });
            var ticket = parkingBoy.Park(parkedCar, out _);
            parkingBoy.Fetch(ticket, out _);

            //when
            var errorMessage = string.Empty;
            parkingBoy.Fetch(ticket, out errorMessage);

            //then
            Assert.Equal("Unrecognized parking ticket.", errorMessage);
        }

        [Fact]
        public void Should_Get_Error_Message_Given_A_Null_Ticket_When_Fetch_Car()
        {
            //given
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });

            //when
            var errorMessage = string.Empty;
            parkingBoy.Fetch(null, out errorMessage);

            //then
            Assert.Equal("Please provide your parking ticket.", errorMessage);
        }

        [Fact]
        public void Should_Get_Error_Message_Given_The_Parking_Lot_Is_Full_When_Park_Car()
        {
            //given
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot(0) });

            //when
            var errorMessage = string.Empty;
            parkingBoy.Park(null, out errorMessage);

            //then
            Assert.Equal("Not enough position.", errorMessage);
        }

        [Fact]
        public void Should_Park_The_Car_To_Mutiple_Parking_Lots_Sequentially()
        {
            //given
            var parkingLot1 = new ParkingLot(1);

            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { parkingLot1, new ParkingLot(2) });

            //when
            var ticket = parkingBoy.Park(new Car("car"), out _);

            //then
            Assert.Contains(ticket.TicketNumber, parkingLot1.Tickets);
        }

        [Fact]
        public void Should_Park_The_Car_To_Mutiple_Parking_Lots_And_Park_To_Next_One_If_Previous_Is_Full()
        {
            //given
            var parkingLot2 = new ParkingLot(2);

            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot(1), parkingLot2 });

            //when
            var ticket1 = parkingBoy.Park(new Car("car1"), out _);
            var ticket2 = parkingBoy.Park(new Car("car2"), out _);

            //then
            Assert.Contains(ticket2.TicketNumber, parkingLot2.Tickets);
        }

        [Fact]
        public void Should_Return_Null_Tickt_When_Park_The_Parked_Car()
        {
            //given
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });
            var car = new Car("car1");
            parkingBoy.Park(car, out _);

            //when
            var ticket = parkingBoy.Park(car, out _);

            //then
            Assert.Null(ticket);
        }

        [Fact]
        public void Should_Return_Null_Tickt_When_Park_The_Null_Car()
        {
            //given
            var parkingBoy = new ParkingBoy(new List<ParkingLot>() { new ParkingLot() });

            //when
            var ticket = parkingBoy.Park(null, out _);

            //then
            Assert.Null(ticket);
        }
    }
}
