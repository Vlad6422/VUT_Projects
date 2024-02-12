// Author: Volodymyr Burylov (xburyl00)
// project: IFJ2023 imperative language translator implementation
#include "scanner.h"

int token_init(Token **token)
{
    (*token) = (Token *)malloc(sizeof(Token));
    if (*token != NULL)
    {
        (*token)->type = TOKEN_NDA;
        return OK;
    }
    return ERROR_INTERNAL;
}

void token_free(Token **token)
{
    if ((*token) != NULL)
    {
        if ((*token)->type == TOKEN_IDE || (*token)->type == TOKEN_STRING)
            ds_free(&(((*token)->attribute).string));
        free(*token);
    }
}

int keyword_or_id(Dynamic_string_t *str, Token *token)
{
    bool flag_nil = false;
    if (!strcmp(str->str, "Int"))
        token->attribute.keyword = KW_INT;
    else if (!strcmp(str->str, "Double"))
        token->attribute.keyword = KW_DOUBLE;
    else if (!strcmp(str->str, "String"))
        token->attribute.keyword = KW_STRING;
    else if (!strcmp(str->str, "else"))
        token->attribute.keyword = KW_ELSE;
    else if (!strcmp(str->str, "func"))
        token->attribute.keyword = KW_FUNCTION;
    else if (!strcmp(str->str, "if"))
        token->attribute.keyword = KW_IF;
    else if (!strcmp(str->str, "let"))
        token->attribute.keyword = KW_LET;
    else if (!strcmp(str->str, "nil"))
        flag_nil = true;
    else if (!strcmp(str->str, "return"))
        token->attribute.keyword = KW_RETURN;
    else if (!strcmp(str->str, "var"))
        token->attribute.keyword = KW_VAR;
    else if (!strcmp(str->str, "while"))
        token->attribute.keyword = KW_WHILE;
    else
    {
        token->type = TOKEN_IDE;
        if (ds_init(&(token->attribute.string)))
            return ERROR_INTERNAL;
        if (ds_copy(str, token->attribute.string))
            return ERROR_INTERNAL;
        return SCAN_END;
    }
    if (flag_nil)
        token->type = TOKEN_NIL;
    else
        token->type = TOKEN_KEY;
    return SCAN_END;
}

int keyword_or_not(Dynamic_string_t *str, Token *token)
{
    if (!strcmp(str->str, "Int"))
        token->attribute.keyword = KW_INT_NIL;
    else if (!strcmp(str->str, "Double"))
        token->attribute.keyword = KW_DOUBLE_NIL;
    else if (!strcmp(str->str, "String"))
        token->attribute.keyword = KW_STRING_NIL;
    else
    {
        token->type = TOKEN_IDE;
        if (ds_init(&(token->attribute.string)))
            return ERROR_INTERNAL;
        if (ds_copy(str, token->attribute.string))
            return ERROR_INTERNAL;
        ds_free(&str);
        return SCAN_END;
    }
    token->type = TOKEN_KEY;
    return SCAN_END;
}

int get_token(Token *token, FILE *File)
{
    if (!File)
    {
        return ERROR_INTERNAL;
    }
    if (token->type == TOKEN_STRING || token->type == TOKEN_IDE)
    {
        ds_free(&(token->attribute.string));
    }
    token->isNLINE = false;
    char State_string_help_str[11];
    strcpy(State_string_help_str, "0x");
    int helpcomblock = 0;
    Dynamic_string_t *S_strin;
    if (ds_init(&S_strin))
    {
        return ERROR_INTERNAL;
    }
    scanner_state state = STATE_START;
    while (true)
    {
        char c = getc(File);
        switch (state)
        {
        case (STATE_START):
            if (c == 9 || c == 13 || c == 32)
            {
                state = STATE_START;
            }
            else if (c == 10)
            {
                token->isNLINE = true;
                state = STATE_START;
            }
            else if (c == '_')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_UND;
            }

            else if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_ID;
            }
            else if (c >= '0' && c <= '9')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_INT;
            }
            else if (c == '"')
            {
                state = STATE_STRING_START;
            }

            else if (c == '/')
            {
                state = STATE_SLASH;
            }

            else if (c == '+')
            {
                ds_free(&S_strin);
                token->type = TOKEN_ADD;
                return SCAN_END;
            }
            else if (c == '*')
            {
                ds_free(&S_strin);
                token->type = TOKEN_MUL;
                return SCAN_END;
            }
            else if (c == '-')
            {
                state = STATE_DASH;
            }
            else if (c == '=')
            {
                state = STATE_EQUAL_SIGN;
            }
            else if (c == '<')
            {
                state = STATE_LT;
            }
            else if (c == '>')
            {
                state = STATE_GT;
            }
            else if (c == ':')
            {
                ds_free(&S_strin);
                token->type = TOKEN_COL;
                return SCAN_END;
            }
            else if (c == '(')
            {
                ds_free(&S_strin);
                token->type = TOKEN_LPAREN;
                return SCAN_END;
            }

            else if (c == ')')
            {
                ds_free(&S_strin);
                token->type = TOKEN_RPAREN;
                return SCAN_END;
            }
            else if (c == '{')
            {
                ds_free(&S_strin);
                token->type = TOKEN_L_CUR_BRACE;
                return SCAN_END;
            }
            else if (c == '}')
            {
                ds_free(&S_strin);
                token->type = TOKEN_R_CUR_BRACE;
                return SCAN_END;
            }
            else if (c == ',')
            {
                ds_free(&S_strin);
                token->type = TOKEN_COMA;
                return SCAN_END;
            }
            else if (c == '!')
            {
                state = STATE_EXC;
            }
            else if (c == '?')
            {
                state = STATE_QUESTION;
            }
            else if (c == EOF)
            {
                ds_free(&S_strin);
                token->type = TOKEN_EOF;
                return SCAN_END;
            }

            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }

            break;

        case (STATE_ID):
            if (c == '_' || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
            }
            else
            {
                if (c == '?')
                {
                    ungetc(c, File);
                    int res = keyword_or_not(S_strin, token);
                    ds_free(&S_strin);
                    if (token->type == TOKEN_KEY)
                    {
                        c = getc(File);
                    }
                    return res;
                }
                else
                {
                    ungetc(c, File);
                    int res = keyword_or_id(S_strin, token);
                    ds_free(&S_strin);
                    return res;
                }
            }
            break;
        case (STATE_UND):
            if (c == '_' || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_ID;
            }
            else
            {
                ungetc(c, File);
                if (ds_init(&(token->attribute.string)))
                    return ERROR_INTERNAL;
                if (ds_copy(S_strin, token->attribute.string))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                token->type = TOKEN_UND;
                ds_free(&S_strin);
                return SCAN_END;
            }
            break;
        case (STATE_INT):
            if (c >= '0' && c <= '9')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
            }
            else if (c == '.')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_DOT;
            }
            else if (c == 'e' || c == 'E')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_EXP;
            }
            else
            {
                ungetc(c, File);
                token->type = TOKEN_INT;
                int H_int = (int)strtol(S_strin->str, NULL, 10);
                token->attribute.integer = H_int;
                ds_free(&S_strin);
                return SCAN_END;
            }
            break;
        case (STATE_DOT):
            if (c >= '0' && c <= '9')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_DOUBLE;
            }
            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;

        case (STATE_DOUBLE):
            if (c >= '0' && c <= '9')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
            }
            else if (c == 'e' || c == 'E')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_EXP;
            }
            else
            {
                ungetc(c, File);
                token->type = TOKEN_DOUBLE;
                double H_double = strtod(S_strin->str, NULL);
                token->attribute.floating = H_double;
                ds_free(&S_strin);
                return SCAN_END;
            }
            break;
        case (STATE_EXP):
            if (c >= '0' && c <= '9')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_EXP_NUM;
            }
            else if (c == '-' || c == '+')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_EXP_ZNAK;
            }
            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;
        case (STATE_EXP_NUM):
            if (c >= '0' && c <= '9')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
            }
            else
            {
                ungetc(c, File);
                token->type = TOKEN_DOUBLE;
                double H_double = strtod(S_strin->str, NULL);
                token->attribute.floating = H_double;
                ds_free(&S_strin);
                return SCAN_END;
            }
            break;
        case (STATE_EXP_ZNAK):
            if (c >= '0' && c <= '9')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_EXP_NUM;
            }
            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;
        case (STATE_STRING_START):
            if (c == '"')
            {
                state = STATE_STRING_START_SEC;
            }
            else if (c == 92)
            {
                state = STATE_BACK;
            }
            else if (c >= 31 && c != 92 && c != '"')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING;
            }
            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;
        case (STATE_STRING_START_SEC):
            if (c == '"')
            {
                state = STATE_STRING_MULT;
            }
            else
            {
                ungetc(c, File);
                if (ds_init(&(token->attribute.string)))
                    return ERROR_INTERNAL;
                if (ds_copy(S_strin, token->attribute.string))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                token->type = TOKEN_STRING;
                ds_free(&S_strin);
                return SCAN_END;
            }
            break;
        case (STATE_STRING_MULT):
            if (c == '"')
            {
                state = STATE_STRING_MULT_FIRST;
            }
            else if (c == EOF)
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            else if (c == '\n')
            {
                token->isNLINE = true;
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
            }
            else
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
            }
            break;
        case (STATE_STRING_MULT_FIRST):
            if (c == '"')
            {
                state = STATE_STRING_MULT_SEC;
            }
            else if (c == EOF)
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            else if (c == '\n')
            {
                ds_add_char(S_strin, '"');
                token->isNLINE = true;
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING_MULT;
            }
            else
            {
                ds_add_char(S_strin, '"');
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING_MULT;
            }
            break;
        case (STATE_STRING_MULT_SEC):
            if (c == '"')
            {
                if (ds_init(&(token->attribute.string)))
                    return ERROR_INTERNAL;
                if (ds_copy(S_strin, token->attribute.string))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                token->type = TOKEN_STRING;
                ds_free(&S_strin);
                return SCAN_END;
            }
            else if (c == EOF)
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            else if (c == '\n')
            {
                ds_add_char(S_strin, '"');
                ds_add_char(S_strin, '"');
                token->isNLINE = true;
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING_MULT;
            }
            else
            {
                ds_add_char(S_strin, '"');
                ds_add_char(S_strin, '"');
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING_MULT;
            }
            break;
        case (STATE_STRING):
            if (c == 92)
            {
                state = STATE_BACK;
            }
            else if (c == '"')
            {
                if (ds_init(&(token->attribute.string)))
                    return ERROR_INTERNAL;
                if (ds_copy(S_strin, token->attribute.string))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                token->type = TOKEN_STRING;
                ds_free(&S_strin);
                return SCAN_END;
            }
            else if (c >= 31 && c != 92 && c != '"')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
            }
            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;
        case (STATE_BACK):
            if (c == '"')
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING;
            }
            else if (c == 'n')
            {
                if (ds_add_char(S_strin, '\n'))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING;
            }
            else if (c == 'r')
            {
                if (ds_add_char(S_strin, 13))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING;
            }
            else if (c == 't')
            {
                if (ds_add_char(S_strin, 9))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING;
            }
            else if (c == 'u')
            {
                state = STATE_STRING_U;
            }
            else if (c == 92)
            {
                if (ds_add_char(S_strin, c))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING;
            }
            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;
        case (STATE_STRING_U):
            if (c == '{')
            {
                state = STATE_STRING_U_NUM;
            }
            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;
        case (STATE_STRING_U_NUM):
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'))
            {
                strncat(State_string_help_str, &c, 1);
            }
            else if (c == '}')
            {
                char *endptr;
                long int H_int = strtol(State_string_help_str, &endptr, 16);
                char buffer[11];
                buffer[0] = (char)H_int;
                buffer[1] = '\0';
                if (ds_add_chars(S_strin, buffer))
                {
                    ds_free(&S_strin);
                    return ERROR_INTERNAL;
                }
                state = STATE_STRING;
            }
            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;
        case (STATE_SLASH):
            if (c == '/')
            {
                state = STATE_COMMENT;
            }
            else if (c == '*')
            {
                helpcomblock++;
                state = STATE_COMMENTBL;
            }
            else
            {
                ungetc(c, File);
                token->type = TOKEN_DIV;
                return SCAN_END;
            }
            break;
        case (STATE_COMMENT):
            if (c == '\n')
            {
                token->isNLINE = true;
                state = STATE_START;
            }
            else if (c == EOF)
            {
                ungetc(c, File);
                state = STATE_START;
            }
            break;
        case (STATE_COMMENTBL):
            if (c == '*')
            {
                state = STATE_COMMENTBL_END;
            }
            else if (c == '/')
            {
                state = STATE_COMMENTBL_ADD;
            }
            else if (c == '\n')
            {
                token->isNLINE = true;
            }
            else if (c == EOF)
            {
                ungetc(c, File);
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;
        case (STATE_COMMENTBL_ADD):
            if (c == '*')
            {
                helpcomblock++;
                state = STATE_COMMENTBL;
            }
            else if (c == '/')
            {
                state = STATE_COMMENTBL_ADD;
            }
            else if (c == '\n')
            {
                token->isNLINE = true;
                state = STATE_COMMENTBL;
            }
            else if (c == EOF)
            {
                ungetc(c, File);
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            else
            {
                state = STATE_COMMENTBL;
            }
            break;
        case (STATE_COMMENTBL_END):
            if (c == '/')
            {
                helpcomblock--;
                if (helpcomblock == 0)
                {
                    state = STATE_START;
                }
                else
                {
                    state = STATE_COMMENTBL;
                }
            }
            else if (c == '\n')
            {
                token->isNLINE = true;
            }
            else if (c == EOF)
            {
                ungetc(c, File);
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            else
            {
                state = STATE_COMMENTBL;
            }
            break;
        case (STATE_EQUAL_SIGN):
            if (c == '=')
            {
                ds_free(&S_strin);
                token->type = TOKEN_EQU;
                return SCAN_END;
            }
            else
            {
                ungetc(c, File);
                ds_free(&S_strin);
                token->type = TOKEN_ASS;
                return SCAN_END;
            }
            break;
        case (STATE_LT):
            if (c == '=')
            {
                token->type = TOKEN_LESSETHAN;
                ds_free(&S_strin);
            }
            else
            {
                ungetc(c, File);
                token->type = TOKEN_LESSTHAN;
                ds_free(&S_strin);
            }
            return SCAN_END;
            break;
        case (STATE_GT):
            if (c == '=')
            {
                token->type = TOKEN_GRETHAN;
                ds_free(&S_strin);
            }
            else
            {
                ungetc(c, File);
                token->type = TOKEN_GRTHAN;
                ds_free(&S_strin);
            }
            return SCAN_END;
            break;
        case (STATE_DASH):
            if (c == '>')
            {
                ds_free(&S_strin);
                token->type = TOKEN_ARROW;
                return SCAN_END;
            }
            else
            {
                ungetc(c, File);
                ds_free(&S_strin);
                token->type = TOKEN_SUB;
                return SCAN_END;
            }
            break;
        case (STATE_QUESTION):
            if (c == '?')
            {
                ds_free(&S_strin);
                token->type = TOKEN_QUESTION;
                return SCAN_END;
            }
            else
            {
                ds_free(&S_strin);
                return ERROR_LEX;
            }
            break;
        case (STATE_EXC):
            if (c == '=')
            {
                ds_free(&S_strin);
                token->type = TOKEN_NOTEQU;
                return SCAN_END;
            }
            else
            {
                ungetc(c, File);
                ds_free(&S_strin);
                token->type = TOKEN_EXC;
                return SCAN_END;
            }
            break;
        }
    }
}