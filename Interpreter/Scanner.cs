using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLox.Interpreter
{
    public class Scanner
    {
        private string Source { get; set; }
        private List<Token> Tokens { get; set; } = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;

        private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            { "and",    TokenType.AND    },
            { "class",  TokenType.CLASS  },
            { "else",   TokenType.ELSE   },
            { "false",  TokenType.FALSE  },
            { "for",    TokenType.FOR    },
            { "fun",    TokenType.FUN    },
            { "if",     TokenType.IF     },
            { "nil",    TokenType.NIL    },
            { "or",     TokenType.OR     },
            { "print",  TokenType.PRINT  },
            { "return", TokenType.RETURN },
            { "super",  TokenType.SUPER  },
            { "this",   TokenType.THIS   },
            { "true",   TokenType.TRUE   },
            { "var",    TokenType.VAR    },
            { "while",  TokenType.WHILE  },
        };


        public Scanner(string source)
        {
            this.Source = source;
        }

        public List<Token> scanTokens()
        {
            while(!isAtEnd())
            {
                start = current;
                scanToken();
            }

            Tokens.Add(new Token(TokenType.EOF, "", null, line));

            return Tokens;
        }

        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '(': addToken(TokenType.LEFT_PAREN); break;
                case ')': addToken(TokenType.RIGHT_PAREN); break;
                case '{': addToken(TokenType.LEFT_BRACE); break;
                case '}': addToken(TokenType.RIGHT_BRACE); break;
                case ',': addToken(TokenType.COMMA); break;
                case '.': addToken(TokenType.DOT); break;
                case '-': addToken(TokenType.MINUS); break;
                case '+': addToken(TokenType.PLUS); break;
                case ';': addToken(TokenType.SEMICOLON); break;
                case '*': addToken(TokenType.STAR); break;
                case '!': addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '=': addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
                case '<': addToken(match('=') ? TokenType.LESS_EQUAL: TokenType.LESS); break;
                case '>': addToken(match('=') ? TokenType.GREATER_EQUAL: TokenType.GREATER); break;
                case '/':
                    if (match('/')) {
                        // Comment goes until the end of the line
                        while (peek() != '\n' && !isAtEnd()) advance();
                    }
                    else if (match('*'))
                    {
                        while(peek() != '/' && !isAtEnd())
                        {
                            if (peek() == '\n') line++;
                            if (peek() == '*' && peekNext() == '/') {
                                break;
                            }
                            advance();
                        }
                        // skip *
                        advance();
                        // skip /
                        advance();
                    }
                    else {
                        addToken(TokenType.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;
                case '\n':
                    line++;
                    break;
                case '"': getString(); break;
                default: 
                    if (isDigit(c)) {
                        number();
                    }
                    else if (isAlpha(c)) {
                        identifier();
                    }
                    else {
                        Lox.error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        private void identifier()
        {
            while (isAlphaNumeric(peek())) advance();

            string text = Source[start..current];
            TokenType type = TokenType.IDENTIFIER;
            if (keywords.ContainsKey(text)) type = keywords[text];
            addToken(type);
        }

        private void number()
        {
            while (isDigit(peek())) advance();

            // Fraction part
            if (peek() == '.' && isDigit(peekNext()))
            {
                // Get the faction
                advance();

                while (isDigit(peek())) advance();
            }

            addToken(TokenType.NUMBER, Double.Parse(Source[start..current]));
        }

        private void getString()
        {
            while (peek() != '"' && !isAtEnd()) {
                if (peek() == '\n') line++;
                advance();
            }

            if (isAtEnd()) {
                Lox.error(line, "Unterminated string.");
                return;
            }

            // Closing "
            advance();

            string value = Source[(start + 1)..(current - 1)];
            addToken(TokenType.STRING, value);
        }

        private bool match(char expected)
        {
            if (isAtEnd()) return false;
            if (Source.ElementAt(current) != expected) return false;

            current++;
            return true;
        }

        private char peek()
        {
            if (isAtEnd()) return '\0';
            return Source.ElementAt(current);
        }

        private char peekNext()
        {
            if (current + 1 >= Source.Length) return '\0';
            return Source.ElementAt(current + 1);
        }

        private bool isAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                   (c >= 'A' && c <= 'Z') ||
                   c == '_';
        }

        private bool isAlphaNumeric(char c)
        {
            return isAlpha(c) || isDigit(c);
        }

        private bool isDigit(char c)
        {
            return c >= '0' && c <= '9';
        }


        private char advance()
        {
            return Source.ElementAt(current++);
        }

        private void addToken(TokenType type)
        {
            addToken(type, null);
        }

        private void addToken(TokenType type, object literal)
        {
            string text = Source[start..current];
            Tokens.Add(new Token(type, text, literal, line));
        }

        private bool isAtEnd()
        {
            return current >= Source.Length;
        }
    }
}
