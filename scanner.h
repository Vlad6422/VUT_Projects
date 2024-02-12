// Author: Volodymyr Burylov (xburyl00)
// project: IFJ2023 imperative language translator implementation
#ifndef TOKEN_H
#define TOKEN_H
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <stdbool.h>
#include "ifj_utils.h"
#include "dynamic_string.h"
typedef enum
{
    STATE_START,
    STATE_ID,
    STATE_UND,
    STATE_INT,
    STATE_DOT,
    STATE_DOUBLE,
    STATE_EXP,
    STATE_EXP_NUM,
    STATE_EXP_ZNAK,
    STATE_STRING_START,
    STATE_STRING_START_SEC,
    STATE_STRING_MULT,
    STATE_STRING_MULT_FIRST,
    STATE_STRING_MULT_SEC,
    STATE_STRING,
    STATE_QUESTION,
    STATE_EXC,
    STATE_BACK,
    STATE_STRING_U,
    STATE_STRING_U_NUM,
    STATE_SLASH,
    STATE_COMMENT,
    STATE_COMMENTBL,
    STATE_COMMENTBL_ADD,
    STATE_COMMENTBL_END,
    STATE_EQUAL_SIGN,
    STATE_LT,
    STATE_GT,
    STATE_DASH,

} scanner_state;

typedef enum
{
    TOKEN_NIL,    // NIL
    TOKEN_INT,    // Integer
    TOKEN_DOUBLE, // Double
    TOKEN_STRING, // String
    TOKEN_KEY,    // keyword
    TOKEN_NUM,    // NUM
    TOKEN_BOOL,   // bool
    TOKEN_IDE,    // identifier type

    TOKEN_QUESTION,    // ??
    TOKEN_UND,         // _
    TOKEN_COMA,        // ,
    TOKEN_DOT,         // .
    TOKEN_COL,         // :
    TOKEN_LPAREN,      // (
    TOKEN_RPAREN,      // )
    TOKEN_L_CUR_BRACE, // {
    TOKEN_R_CUR_BRACE, // }
    TOKEN_EQU,         // ==
    TOKEN_ASS,         // =
    TOKEN_GRTHAN,      // >
    TOKEN_LESSTHAN,    // <
    TOKEN_GRETHAN,     // >=
    TOKEN_LESSETHAN,   // <=
    TOKEN_ARROW,       // ->
    TOKEN_NOTEQU,      // !=
    TOKEN_EXC,         // !
    TOKEN_NDA,

    TOKEN_ADD, // +
    TOKEN_SUB, // -
    TOKEN_MUL, // *
    TOKEN_DIV, // /

    TOKEN_EOF // EOF
} Data_type;

typedef enum
{
    KW_INT,
    KW_INT_NIL,
    KW_DOUBLE,
    KW_DOUBLE_NIL,
    KW_STRING,
    KW_STRING_NIL,
    KW_NUM,
    KW_NIL,
    KW_ELSE,
    KW_FUNCTION,
    KW_IF,
    KW_RETURN,
    KW_LET,
    KW_VAR,
    KW_WHILE
} Keyword;

typedef union
{
    int integer;
    double floating;
    Keyword keyword;
    Dynamic_string_t *string;
} Token_attribute;

typedef struct
{
    Data_type type;
    Token_attribute attribute;
    bool isNLINE;
} Token;

int token_init(Token **token);
void token_free(Token **token);
int get_token(Token *token, FILE *File);

#endif