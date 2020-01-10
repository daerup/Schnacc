﻿namespace Schnacc.Database.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Schnacc.Authorization;

    using Xunit;

    public class DatabaseTest
    {
        private Database testee;

        [Fact]
        private void test()
        {
            // Act
            string email = "hans.muster@mail.ch";
            string password = "testAccount01";
            string signInToken;

            // Act
            this.testee = new Database(new AuthorizationApi().SignInWithEmail(email, password));
            List<Highscore> highscores = this.testee.GetHighscores();

            highscores.Count.Should().Be(2);
            this.testee.WriteHighscore(new Highscore(email, Double.MaxValue));
        }
    }
}
