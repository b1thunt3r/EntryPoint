﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EntryPoint;
using EntryPoint.Exceptions;
using Xunit;
using EntryPointTests.Arguments.AppOptionModels;

namespace EntryPointTests.Arguments {
    public class ListArgumentsTests {
        [Fact]
        public void List_Strings() {
            string[] args = new string[] {
                "--strings", "one,two,three,four"
            };

            var expected = new List<string>() {
                "one", "two", "three", "four"
            };

            var model = Cli.Parse<ListsArgsModel>(args);

            Assert.Equal(expected.Count, model.Strings.Count);
            Assert.True(expected.All(s => model.Strings.Contains(s)));
        }

        [Fact]
        public void List_Ints() {
            string[] args = new string[] {
                "--integers", "1,2,3,4"
            };

            var expected = new List<int>() {
                1, 2, 3, 4
            };

            var model = Cli.Parse<ListsArgsModel>(args);

            Assert.Equal(expected.Count, model.Integers.Count);
            Assert.True(expected.All(s => model.Integers.Contains(s)));
        }

        [Fact]
        public void List_Ints_InvalidType() {
            string[] args = new string[] {
                "--integers", "1,2,3,FAIL"
            };

            Assert.Throws<VariableTypeException>(
                () => Cli.Parse<ListsArgsModel>(args));
        }

        [Fact]
        public void List_Bools() {
            string[] args = new string[] {
                "--booleans", "true,false"
            };

            var expected = new List<bool>() {
                true, false
            };

            var model = Cli.Parse<ListsArgsModel>(args);

            Assert.Equal(expected.Count, model.Booleans.Count);
            Assert.True(expected.All(s => model.Booleans.Contains(s)));
        }

        [Fact]
        public void List_Bools_InvalidType() {
            string[] args = new string[] {
                "--booleans", "true,FAIL"
            };

            Assert.Throws<VariableTypeException>(
                () => Cli.Parse<ListsArgsModel>(args));
        }

        [Fact]
        public void List_Decimals() {
            string[] args = new string[] {
                "--decimals", "1.1,2.1,3.1,4.0"
            };

            var expected = new List<decimal>() {
                1.1m, 2.1m, 3.1m, 4m
            };

            var model = Cli.Parse<ListsArgsModel>(args);

            Assert.Equal(expected.Count, model.Decimals.Count);
            Assert.True(expected.All(s => model.Decimals.Contains(s)));
        }

        [Fact]
        public void List_Decimals_InvalidType() {
            string[] args = new string[] {
                "--decimals", "1.1,2.1,FAIL,4.0"
            };

            Assert.Throws<VariableTypeException>(
                () => Cli.Parse<ListsArgsModel>(args));
        }

        [Fact]
        public void List_Operand_Strings() {
            string[] args = new string[] {
                "one,two,three,four"
            };

            var expected = new List<string>() {
                "one", "two", "three", "four"
            };

            var model = Cli.Parse<ListsOperandsModel>(args);

            Assert.Equal(expected.Count, model.Strings.Count);
            Assert.True(expected.All(s => model.Strings.Contains(s)));
        }

        [Fact]
        public void List_NonStruct() {
            string[] args = new string[] {
                "--class", "param"
            };

            Assert.Throws<InvalidModelException>(
                () => Cli.Parse<ListsNonStructModel>(args));
        }
    }
}
