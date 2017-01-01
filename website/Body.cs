﻿#define CODE

using System;
using System.Linq;

using EntryPoint;

namespace Website {
    class Body {
        /// ### About EntryPoint
        /// 
        /// Entrypoint is an argument parser designed to be composable, practical to use, and maintainable over the life time of a project.
        /// 
        /// It's based around the concept of Declarative POCOs (CliArguments, and CliCommands classes), which you simply pass to EntryPoint for parsing and construction.
        /// 
        /// Parses arguments in the form `UtilityName [command] [-o | --options] [operands]`
        ///
        /// ### Standards
        /// 
        /// * .Net Standard 1.6+ (All future .Net releases are built on this)
        /// * .Net Framework 4.5+
        ///
        /// Follows the [IEEE Standard](http://pubs.opengroup.org/onlinepubs/9699919799/basedefs/V1_chap12.html) for command line utilities 
        /// closely, but does include common adblibs, such as fully named `--option` style options.
        ///
        /// ### Installation
        /// EntryPoint is available on [NuGet](https://www.nuget.org/packages/EntryPoint):
        /// 
        ///     PM> Install-Package EntryPoint
        ///     
    }

    /// ### Introduction
    /// There are just a few classes you'll interact with:
    /// 
    /// * `Cli` - The main API, which handles all processing
    /// * `BaseCliArguments` - An abstract class which you implement to define Options & Operands, known as CliArguments classes
    /// * `BaseCliCommands` - An abstract class which you implement to define Commands, known as CliCommands classes
    /// * `Attributes` - There are a handful of attributes you can use to define your CliCommands and CliArguments implementations, which are documented in depth below
    ///

    /// ## Arguments
    ///
    /// ### Basic Usage
    /// For a simple application you may not need Commands; `CliArguments` classes are used
    /// to parse command line arguments without consideration of Commands.
    /// 
    /// Let's say we want a utility used like: `UtilityName [-s] [--name Bob] [6.1]`
    /// 
    /// This has one Option (-s), one OptionParameter (--name Bob) and a positional Operand (6.1)
#if CODE
    class SimpleProgram {
        void main(string[] args) {

            // One line parsing of any `BaseCliArguments` implementation
            var arguments = Cli.Parse<SimpleCliArguments>(args);

            // Object oriented access to your arguments
            Console.WriteLine($"The name is {arguments.Name}");
            Console.WriteLine($"Switch flag: {arguments.Switch}");
            Console.WriteLine($"Positional Operand 1: {arguments.FirstOperand}");
        }
    }

    class SimpleCliArguments : BaseCliArguments {
        public SimpleCliArguments() : base("SimpleApp") { }

        // Defining a CliArguments class is as easy as adding 
        // attributes to your POCO properties
        [Option(LongName = "switch", 
                ShortName = 's')]
        public bool Switch { get; set; }

        // Both Option and OptionParameter attributes support a 
        // combination of -o and --option style invocations
        [OptionParameter(LongName = "name", 
                         ShortName = 'n')]
        public string Name { get; set; }

        // Operands can be mapped positionally
        // But BaseCliArguments also has a .Operands string[] where 
        // un-mapped operands are stored
        [Operand(position: 1)]
        public decimal FirstOperand { get; set; }
    }
#endif

    /// ### CliArguments Attributes
    /// 
    /// We use Attributes to define CLI functionality
    /// 
    /// ##### `[Option(LongName = string, ShortName = char)]`
    /// * **Apply to:** Class Properties
    /// * **Output Types:** Bool
    /// * **Detail:** Defines an On/Off option for use on the CLI
    /// * **Argument, LongName:** the case in-sensitive name to be used like `--name`
    /// * **Argument, ShortName:** the case sensitive character to be used like `-n`
    /// * At least one name needs to be provided
    /// 
    /// ##### `[OptionParameter(LongName = string, ShortName = char)]`
    /// * **Apply to:** Class Properties
    /// * **Output Types:** Primitive Types, Enums
    /// * **Detail:** Defines a parameter which can be invoked to provide a value
    /// * **Argument, LongName:** the case in-sensitive name to be used like `--name`
    /// * **Argument, ShortName:** the case sensitive character to be used like `-n`
    /// * At least one name needs to be provided
    /// 
    /// ##### `[Operand(position = int)]`
    /// * **Apply to:** Class Properties
    /// * **Output Types:** Primitive Types, Enums
    /// * **Detail:** Maps a positional operand from the end of a CLI command
    /// * **Argument, Position:** the 1 based position of the Operand
    /// 
    /// ##### `[Required]`
    /// * **Apply to:** Option, OptionParameter or Operand properties
    /// * **Detail:** Makes an Option or Operand mandatory for the user to provide
    /// 
    /// ##### `[Help(detail = string)]`
    /// * **Apply to:** Class Properties with any Option or Operand Attribute applied, or an CliArguments Class
    /// * **Detail:** Provides custom documentation on an Option, Operand or CliArguments Class, which will be consumed by the help generator
    ///


    /// ### Example Application
    /// 
    /// The following is an example implementation for use in a simple message sending application
    /// 
    /// This is used like `UtilityName [ -v | --verbose ] [ -s | --subject "your subject" ] [ -i | --importance [ normal | high ] ] [message]`
#if CODE
    // Usage is as simple as
    class MessagingProgram {
        void main(string[] args) {
            var arguments = Cli.Parse<MessagingCliArguments>(args);

            // Use the arguments object...
        }
    }

    class MessagingCliArguments : BaseCliArguments {
        public MessagingCliArguments() : base("Message Sender") { }

        // Verbose will be a familiar option to most CLI users
        [Option(LongName = "verbose", 
                ShortName = 'v')]
        [Help("When this is set, verbose logging will be activated")]
        public bool Verbose { get; set; }

        // A subject *must* be provided by the user 
        [Required]
        [OptionParameter(LongName = "subject",
                         ShortName = 's')]
        [Help("Mandatory Subject to provide")]
        public string Subject { get; set; }

        // An importance level for the message.
        // If not provided this is defaulted to `Normal`
        // User can provide the value as a number or string (ie. '2' or 'high')
        [OptionParameter(LongName = "importance", 
                         ShortName = 'i')]
        [Help("Sets the importance level of a sent message")]
        public MessageImportanceEnum Importance { get; set; } = MessageImportanceEnum.Normal;

        // A message *must* be provided as the first operand
        [Required]
        [Operand(1)]
        [Help("Mandatory message to provide")]
        public string Message { get; set; }
    }

    enum MessageImportanceEnum {
        Normal = 1,
        High = 2
    }
#endif

    /// ### Value Defaults
    /// 
    /// If the user does not provide an non-required option-parameter or operand, 
    /// it can be useful to configure the application with a default.
    /// 
    /// This is easily done using C# property initialisers, 
    /// and will otherwise use the type's default value
    /// 
    class DefaultsExample {
#if CODE
        // The following Importance Enum will always be set to 'Normal'
        // if the user does not provide a value
        [OptionParameter(LongName = "importance",
                         ShortName = 'i')]
        [Help("Sets the importance level of a sent message")]
        public MessageImportanceEnum Importance { get; set; } = MessageImportanceEnum.Normal;
#endif
    }
    
    /// ## Commands
    /// 
    /// ### Introduction
    /// 
    /// Although it's perfectly fine to only use a CliArguments class for a simple application, 
    /// if you have multiple Commands, each with a different set of Arguments; you may want
    /// to create multiple application entry points, each with its own CliArguments class.
    /// 
    /// This is the purpose of `BaseCliCommands`.
    /// 
    /// ### Basic Usage
#if CODE
    class SimpleCommandsProgram {
        public void Main(string[] args) {
            // One line execution of the Commands class
            // It will select and route to one of your Command methods
            var commands = Cli.Execute<SimpleCliCommands>(args);
        }
    }

    class SimpleCliCommands : BaseCliCommands {

        // A command is a Method which takes a `string[]`.
        // You also need to apply a [Command(name)] attribute, 
        // with the name of the command on the CLI
        [Command("command1")]
        public void Command1(string[] args) {
            // var arguments = Cli.Parse<Command1CliArguments>(args);
            // ...Application logic
        }

        // You can also define a Default command.
        // This helps if you want a fallback when the user doesn't name a command
        [DefaultCommand]
        [Command("command2")]
        public void Command2(string[] args) {
            // var arguments = Cli.Parse<Command2CliArguments>(args);
            // ...Application logic
        }
    }
#endif

    /// ### CliCommands Attributes
    /// 
    /// There are several attributes which can be applied to a CliCommands class
    /// 
    /// ##### `[Command(Name = string)]`
    /// * **Apply to:** Methods with the signature: `void MethodName(string[])`
    /// * **Argument, Name:** This is the Command Name to be used on the CLI like: `Utility [Command Name] [options]`
    /// * **Detail:** Defines a method as a Command to be routed to
    /// 
    /// ##### `[DefaultCommand]`
    /// * **Apply to:** Command m\ethods
    /// * **Detail:** Defines a Command as the default when no Command is specified, otherwise EntryPoint invokes `--help`
    /// 
    /// ##### `[Help(detail = string)]`
    /// * **Apply to:** CliCommands classes and Command methods
    /// * **Detail:** Provides custom documentation on a Command



    /// ## Help Generator
    /// 
    /// ### Introduction
    /// 
    /// EntryPoint provides an automatic Help generator, which always owns the `-h` and `--help` 
    /// Options in both CliCommands and CliArguments instances.
    /// 
    /// When `--help` is invoked by the user, `.HelpInvoked` is set on CliCommands/CliArguments,  
    /// and the empty virtual method `OnHelpInvoked(string helpText)` is invoked.
    /// 
    /// By overriding `OnHelpInvoked` on your CliCommands/CliArguments implementations,
    /// you can print and exit, or do something more appropriate to your program flow. 
    /// 
    /// EntryPoint does not try to control your usage of this, but be aware that invoking `--help` will 
    /// disable  `Required` attributes; if you neither exit or check `.HelpInvoked`, then your 
    /// program may continue running with invalid state.
    ///
    /// ### Basic Usage
    /// 
    /// The Help Generator consumes the following information for each class type.
    /// 
    /// CliCommands
#if CODE
    [Help("This will be displayed as an initial blurb for the utility")]
    class HelpCliCommands : BaseCliCommands {

        // [DefaultCommand]
        // [Command(...)]
        [Help("Displayed as instructions for a command")]
        public void CommandMethod(string[] args) {
            // ...
        }
    }
#endif
    /// CliArguments
#if CODE
    [Help("This will be displayed as an initial blurb for the command/utility")]
    class HelpCliArguments : BaseCliArguments {
        public HelpCliArguments()
            : base(utilityName: "Displayed as the command/utility name") { }

        // [Option(...)]
        // [OptionParameter(...)]
        // [Operand(...)]
        [Help("Displayed as additional instructions for an Option/Operand")]
        public bool Value { get; set; }
    }
#endif

    /// A simple implementation would therefore look like this:
#if CODE
    [Help("This will be displayed as an initial blurb for the utility")]
    class ExampleHelpCliCommands : BaseCliCommands {

        // [DefaultCommand]
        [Command("command1")]
        [Help("Some command that can be used")]
        public void Command1(string[] args) {
            // ...etc
        }

        // This will run if --help is invoked, print help and exit the program
        public override void OnHelpInvoked(string helpText) {
            Console.WriteLine(helpText);
            Environment.Exit(0);
        }
    }

    class CommandsHelpProgram {
        public void main(string[] args) {
            var commands = Cli.Execute<ExampleHelpCliCommands>(args);
            // Execution would not reach this point if --help is invoked, 
            // since OnHelpInvoked would run and exit the program

            // However, if you don't want to implement OnHelpInvoked, 
            // you could also do this:
            if (commands.HelpInvoked) {
                // Return here, or run something else
            }

            // Normal Post-Command Application code...
        }
    }
#endif

    /// ...and the same thinking applies to CliArguments
#if CODE
    [Help("This will be displayed as an initial blurb for the command/utility")]
    class ExampleHelpCliArguments : BaseCliArguments {
        public ExampleHelpCliArguments()
            : base(utilityName: "Displayed as the command/utility name") { }
        
        [OptionParameter(LongName = "value1")]
        [Help("Some value to set")]
        public bool Value1 { get; set; }

        public override void OnHelpInvoked(string helpText) {
            Console.WriteLine(helpText);
            Environment.Exit(0);
        }
    }

    class ArgumentsHelpProgram {
        public void main(string[] args) {
            var arguments = Cli.Parse<ExampleHelpCliArguments>(args);
            // Execution would not reach this point if --help is invoked, 
            // since OnHelpInvoked would run and exit the program

            // However, if you don't want to implement OnHelpInvoked, 
            // you could also do this:
            if (arguments.HelpInvoked) {
                // Return here, or run something else
            }

            // Normal Application code...
        }
    }
#endif

    /// ## Tips & Behaviour
    /// 
    /// * `Cli.Parse` and `Cli.Execute` have several overloads available. They can create the class and get the command line arguments themselves, but give you manual control, too.
    /// * Short named options `-o` are case sensitive: `-a != -A`
    /// * Long named options `--option` are case insensitive: `--opt == --Opt`
    /// * Options can be combined by the user: `-a -b -c` -> `-abc`
    /// * Combined options can end with an option parameter: `-abco value`
    /// * Option-parameters have several forms: `-o value` `-o=value` `--option value` `--option=value`
    /// * Quotes and Escape characters are both supported: `--option "my value"` `--option \-my-value`
    /// * **Warning:** be careful with Quotes as .Net respects and then removes them during `string[] args` creation. They can be escaped to include in values.
}