// Author: Malashchuk Vladyslav (xmalas04)
// project: IFJ2023 imperative language translator implementation
#include "expression.h"
#include "stack.h"

#define TABLE_SIZE 9

// Precedence table
int precedence_table[TABLE_SIZE][TABLE_SIZE] =
    {
        //	|+- | */| ! | ??| r | ( | ) | i | $ |
        {R, S, S, R, R, S, R, S, R}, /// +-
        {R, R, S, R, R, S, R, S, R}, /// */
        {R, R, N, R, R, S, R, N, R}, /// !
        {S, S, S, S, S, S, R, S, R}, /// ??
        {S, S, S, R, N, S, R, S, R}, /// r (realtion operators) == != <= < >= >
        {S, S, S, S, S, S, E, S, N}, /// (
        {R, R, R, R, R, N, R, N, R}, /// )
        {R, R, R, R, R, N, R, N, R}, /// i (id, int, double, string)
        {S, S, S, S, S, S, N, S, N}  /// $
};
Stack st;
Stack *stack = &st;
/**
 * @brief Auxiliary function, identifies if there is relevant rule for up to 3 top symbols
 * @param count number of stack items before STOP sign
 * @param item1 top item
 * @param item2 next after top
 * @param item3 next after top after top
 * @return Relevant rule
 */
pt_rule test_rule(int count, Stack_item *item1, Stack_item *item2, Stack_item *item3);

/**
 * Function converts symbol to precedence table index.
 *
 * @param symbol Symbol to be converted.
 * @return Returns precedence table index.
 */
static pt_index get_prec_table_index(pt_symbol symbol)
{
    switch (symbol)
    {
    case PLUS:
    case MINUS:
        return I_PLUS_MINUS;

    case MUL:
    case DIV:
        return I_MUL_DIV;

    case NEG:
        return I_NEG;
    case Nil_Coalescing_Operator:
        return I_NilCoalescingOperator;

    case EQ:
    case NEQ:
    case LEQ:
    case LTN:
    case MEQ:
    case MTN:
        return I_REL_OP;

    case LEFT_BRACKET:
        return I_LEFT_BRACKET;

    case RIGHT_BRACKET:
        return I_RIGHT_BRACKET;

    case IDENTIFIER:
    case INT_NUMBER:
    case DOUBLE_NUMBER:
    case STRING:
    case NIL:
        return I_DATA;

    default:
        return I_DOLLAR;
    }
}
/**
 * Function converts token type to symbol.
 *
 * @param token Pointer to token.
 * @return Returns dollar if symbol is not supported or converted symbol if symbol is supported.
 */
static pt_symbol get_symbol_from_token(Token *token)
{
    switch (token->type)
    {
    case TOKEN_ADD:
        return PLUS;
    case TOKEN_SUB:
        return MINUS;
    case TOKEN_MUL:
        return MUL;
    case TOKEN_DIV:
        return DIV;
    case TOKEN_NOTEQU:
        return NEQ;
    case TOKEN_EQU:
        return EQ;
    case TOKEN_QUESTION:
        return Nil_Coalescing_Operator;
    case TOKEN_EXC:
        return NEG;
    case TOKEN_LESSETHAN:
        return LEQ;
    case TOKEN_LESSTHAN:
        return LTN;
    case TOKEN_GRETHAN:
        return MEQ;
    case TOKEN_GRTHAN:
        return MTN;
    case TOKEN_LPAREN:
        return LEFT_BRACKET;
    case TOKEN_RPAREN:
        return RIGHT_BRACKET;
    case TOKEN_IDE:
    case TOKEN_ASS:
        return IDENTIFIER;
    case TOKEN_NIL:
        return NIL;

    case TOKEN_INT:
        return INT_NUMBER;
    case TOKEN_DOUBLE:
        return DOUBLE_NUMBER;
    case TOKEN_STRING:
        return STRING;
    default:
        return DOLLAR;
    }
}
/**
 * Shift - push Stop before terminal and push symbol with type
 *
 * @param parser_data Pointer to Parser_utils_t.
 * @param type Data Type
 * @param symbol Symbol
 * @return Returns 0 (OK) or 5 (ERROR_UNDEF_OR_UNINIT_VAR)
 */
int shift(Parser_utils_t *parser_data, Stack *stack, Data_type type, pt_symbol symbol)
{
    s_push_before_terminal(stack, TOKEN_NDA, STOP);
    s_push(stack, type, symbol);
    if (!expect_token_type(parser_data->parser_token, TOKEN_IDE))
    {
        parser_data->var = search_on_all_scope(parser_data->stack_sym_tab->top, parser_data->parser_token->attribute.string);
        if (parser_data->var == NULL)
        {
            fprintf(stderr, "ERRORCODE : %d!\n UNDEFVAR", ERROR_UNDEF_OR_UNINIT_VAR);
            return ERROR_UNDEF_OR_UNINIT_VAR;
        }
    }
    return 0;
}

Data_type convert_token2type(Parser_utils_t *parser_data)
{
    Symbol_t *data;

    switch (parser_data->parser_token->type)
    {

    case (TOKEN_INT):
        return TOKEN_INT;
    case (TOKEN_DOUBLE):
        return TOKEN_DOUBLE;
    case (TOKEN_STRING):
        return TOKEN_STRING;
    case (TOKEN_NIL):
        return TOKEN_NIL;
    case (TOKEN_IDE):
        data = search_on_all_scope(parser_data->stack_sym_tab->top, parser_data->parser_token->attribute.string);
        if (data == NULL)
        {
            return ERROR_UNDEF_OR_UNINIT_VAR;
        }
        if (data->sym_class == SYMBOL_FUNCTION)
        {
            fprintf(stderr, "Error code: %d! Expression: Function.\n", ERROR_UNDEF_OR_UNINIT_VAR);
            return ERROR_UNDEF_OR_UNINIT_VAR;
        }
        if (!data->var_init)
        {
            fprintf(stderr, "Error code: %d! Expression: Not init var.\n", ERROR_UNDEF_OR_UNINIT_VAR);
            return ERROR_UNDEF_OR_UNINIT_VAR;
        }

        if (data->type == IFJ_DOUB)
            return (Data_type)VAR_DOUBLE;
        else if (data->type == IFJ_INT)
            return (Data_type)VAR_INT;
        else if (data->type == IFJ_STR)
            return (Data_type)VAR_STRING;
        else if (data->type == IFJ_INT_NIL)
            return (Data_type)VAR_INT_NIL;
        else if (data->type == IFJ_DOUB_NIL)
            return (Data_type)VAR_DOUBLE_NIL;
        else if (data->type == IFJ_NIL)
            return (Data_type)TOKEN_NIL;
        else if (data->type == IFJ_STR_NIL)
            return (Data_type)VAR_STRING_NIL;
        break;
    default:
        return TOKEN_NDA;
        break;
    }
}
/**
 * @brief Semantic Checker
 * @param item1 top item
 * @param item2 next after top
 * @param item3 next after top after top
 * @return Result type of operation
 */
Data_type test_semantics(Stack_item *item1, Stack_item *item2, Stack_item *item3, pt_rule rule)
{
    switch (rule)
    {
    case E_RULE: // DONE
        return item1->type;
        break;
    case E_NEG: // DONE but need to think Postfixový unární operátor ! slouží pro úpravu typu operandu tak, že na zodpovědnost programátora ve výsledku nemůže být nil (jinak může dojít k běhové chybě)
        if (item2->type == TOKEN_NIL)

            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
        else if (item2->type == (Data_type)VAR_INT_NIL)
            return (Data_type)VAR_INT;
        else if (item2->type == (Data_type)VAR_DOUBLE_NIL)
            return (Data_type)VAR_DOUBLE;
        else if (item2->type == (Data_type)VAR_STRING_NIL)
            return (Data_type)VAR_STRING;
        return item2->type;
        break;

    case E_PLUS_E: // DONE
        if ((item1->type == TOKEN_NIL || item1->type == (Data_type)VAR_DOUBLE_NIL || item1->type == (Data_type)VAR_INT_NIL || item1->type == (Data_type)VAR_STRING_NIL) || (item3->type == TOKEN_NIL || item3->type == (Data_type)VAR_DOUBLE_NIL || item3->type == (Data_type)VAR_INT_NIL || item3->type == (Data_type)VAR_STRING_NIL))
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
        else if (item1->type == TOKEN_STRING && item3->type == TOKEN_STRING)
        {
            return TOKEN_STRING;
        }
        else if (item1->type == TOKEN_STRING && item3->type == (Data_type)VAR_STRING)
        {
            return (Data_type)VAR_STRING;
        }
        else if (item1->type == (Data_type)VAR_STRING && item3->type == TOKEN_STRING)
        {
            return (Data_type)VAR_STRING;
        }
        else if (item1->type == (Data_type)VAR_STRING && item3->type == (Data_type)VAR_STRING)
        {
            return (Data_type)VAR_STRING;
        }

    case E_MINUS_E: // DONE
    case E_MUL_E:
        if ((item1->type == TOKEN_NIL || item1->type == (Data_type)VAR_DOUBLE_NIL || item1->type == (Data_type)VAR_INT_NIL || item1->type == (Data_type)VAR_STRING_NIL) || (item3->type == TOKEN_NIL || item3->type == (Data_type)VAR_DOUBLE_NIL || item3->type == (Data_type)VAR_INT_NIL || item3->type == (Data_type)VAR_STRING_NIL))
        {
            fprintf(stderr, "Error code: %d! Expression: Using Nil var.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
        }
        else if (item1->type == TOKEN_INT && item3->type == TOKEN_INT)
        {
            return TOKEN_INT;
        } // Jsou-li oba operandy typu Int, je i výsledek typu Int
        else if (item1->type == TOKEN_DOUBLE && item3->type == TOKEN_DOUBLE)
            return TOKEN_DOUBLE; // Jsou-li oba operandy typu Double, výsledek je typu Double.

        else if (item1->type == (Data_type)VAR_INT && item3->type == (Data_type)VAR_INT)
            return (Data_type)VAR_INT; // Jsou-li oba operandy typu Int, je i výsledek typu Int
        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == (Data_type)VAR_DOUBLE)
            return (Data_type)VAR_DOUBLE; // Jsou-li oba operandy typu Double, výsledek je typu Double.

        else if (item1->type == TOKEN_INT && item3->type == TOKEN_DOUBLE)
            return TOKEN_DOUBLE; // INT + DOUBLE = DOUBLE
        else if (item1->type == TOKEN_DOUBLE && item3->type == TOKEN_INT)
            return TOKEN_DOUBLE; // INT + DOUBLE = DOUBLE

        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == TOKEN_INT)
            return (Data_type)VAR_DOUBLE; // INT + VAR_DOUBLE = VAR_ DOUBLE
        else if (item1->type == TOKEN_INT && item3->type == (Data_type)VAR_DOUBLE)
            return (Data_type)VAR_DOUBLE; // INT + VAR_DOUBLE = VAR_ DOUBLE

        else if (item1->type == (Data_type)VAR_INT && item3->type == TOKEN_INT)
            return (Data_type)VAR_INT; // INT + VAR_INT = VAR_INT
        else if (item1->type == TOKEN_INT && item3->type == (Data_type)VAR_INT)
            return (Data_type)VAR_INT; // INT + VAR_INT = VAR_INT

        else if (item1->type == (Data_type)VAR_INT && item3->type == TOKEN_DOUBLE)
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR); // Na proměnné se v IFJ23 ani ve Swift implicitní konverze neaplikují
        else if (item1->type == TOKEN_DOUBLE && item3->type == (Data_type)VAR_INT)
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR); // Na proměnné se v IFJ23 ani ve Swift implicitní konverze neaplikují

        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == (Data_type)VAR_INT)
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR); // Na proměnné se v IFJ23 ani ve Swift implicitní konverze neaplikují
        else if (item1->type == (Data_type)VAR_INT && item3->type == (Data_type)VAR_DOUBLE)
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR); // Na proměnné se v IFJ23 ani ve Swift implicitní konverze neaplikují

        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == TOKEN_DOUBLE)
            return (Data_type)VAR_DOUBLE; // VAR_DOUBLE + DOUBLE =  VAR_DOUBLE
        else if (item1->type == TOKEN_DOUBLE && item3->type == (Data_type)VAR_DOUBLE)
            return (Data_type)VAR_DOUBLE; // VAR_DOUBLE + DOUBLE =  VAR_DOUBLE
        return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        break;
    case E_DIV_E: // DONE
        if ((item1->type == TOKEN_NIL || item1->type == (Data_type)VAR_DOUBLE_NIL || item1->type == (Data_type)VAR_INT_NIL || item1->type == (Data_type)VAR_STRING_NIL) || (item3->type == TOKEN_NIL || item3->type == (Data_type)VAR_DOUBLE_NIL || item3->type == (Data_type)VAR_INT_NIL || item3->type == (Data_type)VAR_STRING_NIL))
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
        else if (item1->type == TOKEN_INT && item3->type == TOKEN_INT)
            return TOKEN_INT;
        else if (item1->type == TOKEN_DOUBLE && item3->type == TOKEN_DOUBLE)
            return TOKEN_INT;
        else if (item1->type == (Data_type)VAR_INT && item3->type == (Data_type)VAR_INT)
            return (Data_type)VAR_INT;
        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == (Data_type)VAR_DOUBLE)
            return (Data_type)VAR_DOUBLE;
        else if (item1->type == TOKEN_DOUBLE && item3->type == (Data_type)VAR_DOUBLE)
            return (Data_type)VAR_DOUBLE;
        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == TOKEN_DOUBLE)
            return (Data_type)VAR_DOUBLE;
        else if (item1->type == (Data_type)VAR_INT && item3->type == TOKEN_INT)
            return (Data_type)VAR_INT;
        else if (item1->type == TOKEN_INT && item3->type == (Data_type)VAR_INT)
            return (Data_type)VAR_INT;
        return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
    case LBR_E_RBR: // DONE

        if (item2->type == TOKEN_NIL)
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);

        return item2->type;
        break;
    case E_LTH_E: // MUST BE ONE TYPE
    case E_LET_E:
    case E_MTH_E:
    case E_MET_E:
        if (item1->type == TOKEN_STRING && item3->type == TOKEN_STRING)
        {
            return TOKEN_BOOL;
        }
        else if (item1->type == TOKEN_STRING && item3->type == (Data_type)VAR_STRING)
        {
            return TOKEN_BOOL;
        }
        else if (item1->type == (Data_type)VAR_STRING && item3->type == TOKEN_STRING)
        {
            return TOKEN_BOOL;
        }
        else if (item1->type == (Data_type)VAR_STRING && item3->type == (Data_type)VAR_STRING)
        {
            return TOKEN_BOOL;
        }
        else if (item1->type == TOKEN_INT && item3->type == TOKEN_INT)
        {
            return TOKEN_BOOL;
        } // Jsou-li oba operandy typu Int, je i výsledek typu Int
        else if (item1->type == TOKEN_DOUBLE && item3->type == TOKEN_DOUBLE)
            return TOKEN_BOOL; // Jsou-li oba operandy typu Double, výsledek je typu Double.
        else if (item1->type == (Data_type)VAR_INT && item3->type == (Data_type)VAR_INT)
            return TOKEN_BOOL; // Jsou-li oba operandy typu Int, je i výsledek typu Int
        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == (Data_type)VAR_DOUBLE)
            return TOKEN_BOOL; // Jsou-li oba operandy typu Double, výsledek je typu Double.
        else if (item1->type == TOKEN_INT && item3->type == TOKEN_DOUBLE)
            return TOKEN_BOOL; // INT + DOUBLE = DOUBLE
        else if (item1->type == TOKEN_DOUBLE && item3->type == TOKEN_INT)
            return TOKEN_BOOL; // INT + DOUBLE = DOUBLE
        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == TOKEN_INT)
            return TOKEN_BOOL; // INT + VAR_DOUBLE = VAR_ DOUBLE
        else if (item1->type == TOKEN_INT && item3->type == (Data_type)VAR_DOUBLE)
            return TOKEN_BOOL; // INT + VAR_DOUBLE = VAR_ DOUBLE
        else if (item1->type == (Data_type)VAR_INT && item3->type == TOKEN_INT)
            return TOKEN_BOOL; // INT + VAR_INT = VAR_INT
        else if (item1->type == TOKEN_INT && item3->type == (Data_type)VAR_INT)
            return TOKEN_BOOL; // INT + VAR_INT = VAR_INT
        else if (item1->type == (Data_type)VAR_INT && item3->type == TOKEN_DOUBLE)
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR); // Na proměnné se v IFJ23 ani ve Swift implicitní konverze neaplikují
        else if (item1->type == TOKEN_DOUBLE && item3->type == (Data_type)VAR_INT)
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR); // Na proměnné se v IFJ23 ani ve Swift implicitní konverze neaplikují
        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == (Data_type)VAR_INT)
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR); // Na proměnné se v IFJ23 ani ve Swift implicitní konverze neaplikují
        else if (item1->type == (Data_type)VAR_INT && item3->type == (Data_type)VAR_DOUBLE)
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR); // Na proměnné se v IFJ23 ani ve Swift implicitní konverze neaplikují
        else if (item1->type == (Data_type)VAR_DOUBLE && item3->type == TOKEN_DOUBLE)
            return TOKEN_BOOL; // VAR_DOUBLE + DOUBLE =  VAR_DOUBLE
        else if (item1->type == TOKEN_DOUBLE && item3->type == (Data_type)VAR_DOUBLE)
            return TOKEN_BOOL; // VAR_DOUBLE + DOUBLE =  VAR_DOUBLE
        else if (item1->type == (Data_type)VAR_INT_NIL && item3->type == TOKEN_INT)
            return TOKEN_BOOL;
        else if (item3->type == (Data_type)VAR_INT_NIL && item1->type == TOKEN_INT)
            return TOKEN_BOOL;
        else if (item1->type == (Data_type)VAR_DOUBLE_NIL && item3->type == TOKEN_DOUBLE)
            return TOKEN_BOOL;
        else if (item3->type == (Data_type)VAR_DOUBLE_NIL && item1->type == TOKEN_DOUBLE)
            return TOKEN_BOOL;
        else if (item1->type == (Data_type)VAR_STRING_NIL && item3->type == TOKEN_STRING)
            return TOKEN_BOOL;
        else if (item3->type == (Data_type)VAR_STRING_NIL && item1->type == TOKEN_STRING)
            return TOKEN_BOOL;
        return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        break;

    case E_EQU_E:
    case E_NEQ_E:
        if (item1->type == TOKEN_INT)
        {
            if (item3->type == TOKEN_INT)
                return TOKEN_BOOL;
            if (item3->type == (Data_type)VAR_INT)
                return TOKEN_BOOL;
        }
        if (item1->type == (Data_type)VAR_INT)
        {
            if (item3->type == TOKEN_INT)
                return TOKEN_BOOL;
            if (item3->type == (Data_type)VAR_INT)
                return TOKEN_BOOL;
        }
        if (item1->type == TOKEN_DOUBLE)
        {
            if (item3->type == TOKEN_DOUBLE)
                return TOKEN_BOOL;
            if (item3->type == (Data_type)VAR_DOUBLE)
                return TOKEN_BOOL;
        }
        if (item1->type == (Data_type)VAR_DOUBLE)
        {
            if (item3->type == TOKEN_DOUBLE)
                return TOKEN_BOOL;
            if (item3->type == (Data_type)VAR_DOUBLE)
                return TOKEN_BOOL;
        }
        if (item1->type == TOKEN_STRING)
        {
            if (item3->type == TOKEN_STRING)
                return TOKEN_BOOL;
            if (item3->type == (Data_type)VAR_STRING)
                return TOKEN_BOOL;
        }
        if (item1->type == (Data_type)VAR_STRING)
        {
            if (item3->type == TOKEN_STRING)
                return TOKEN_BOOL;
            if (item3->type == (Data_type)VAR_STRING)
                return TOKEN_BOOL;
        }
        if (item1->type == (Data_type)VAR_INT_NIL && item3->type == (Data_type)VAR_INT_NIL)
            return TOKEN_BOOL;
        if (item1->type == (Data_type)VAR_DOUBLE_NIL && item3->type == (Data_type)VAR_DOUBLE_NIL)
            return TOKEN_BOOL;
        if (item1->type == (Data_type)VAR_STRING_NIL && item3->type == (Data_type)VAR_STRING_NIL)
            return TOKEN_BOOL;
        if (item1->type == TOKEN_NIL)
        {
            if (item3->type != TOKEN_BOOL)
                return TOKEN_BOOL;
        }
        else if (item3->type == TOKEN_NIL)
        {
            if (item1->type != TOKEN_BOOL)
                return TOKEN_BOOL;
        }
        return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        break;
    case E_NIL_E:
        if (item1->type == TOKEN_NIL) // ??????
            return (ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
        else if (item3->type == TOKEN_NIL)
            return item1->type;
        else if (item3->type == TOKEN_INT)
        {
            if (item1->type == TOKEN_INT || item1->type == (Data_type)VAR_INT)
                return TOKEN_INT;
            else
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        else if (item3->type == TOKEN_DOUBLE)
        {
            if (item1->type == TOKEN_DOUBLE || item1->type == (Data_type)VAR_DOUBLE)
                return TOKEN_DOUBLE;
            else
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        else if (item3->type == TOKEN_STRING)
        {
            if (item1->type == TOKEN_STRING || item1->type == (Data_type)VAR_STRING)
                return TOKEN_STRING;
            else
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        else if (item3->type == (Data_type)VAR_INT)
        {
            if (item1->type == TOKEN_INT || item1->type == (Data_type)VAR_INT)
                return (Data_type)VAR_INT;
            else
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        else if (item3->type == (Data_type)VAR_DOUBLE)
        {
            if (item1->type == TOKEN_DOUBLE || item1->type == (Data_type)VAR_DOUBLE)
                return (Data_type)VAR_DOUBLE;
            else
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        else if (item3->type == (Data_type)VAR_STRING)
        {
            if (item1->type == TOKEN_STRING || item1->type == (Data_type)VAR_STRING)
                return (Data_type)VAR_STRING;
            else
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        else if (item3->type == (Data_type)VAR_INT_NIL)
        {
            if (item1->type == (Data_type)VAR_INT)
                return (Data_type)VAR_INT;
            else if (item1->type == TOKEN_INT)
                return TOKEN_INT;
            else
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        else if (item3->type == (Data_type)VAR_DOUBLE_NIL)
        {
            if (item1->type == (Data_type)VAR_DOUBLE)
                return (Data_type)VAR_DOUBLE;
            else if (item1->type == TOKEN_DOUBLE)
                return TOKEN_DOUBLE;
            else
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        else if (item3->type == (Data_type)VAR_STRING_NIL)
        {
            if (item1->type == (Data_type)VAR_STRING)
                return (Data_type)VAR_STRING;
            else if (item1->type == TOKEN_STRING)
                return TOKEN_STRING;
            else
                return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }

        break;
    default:
        return item1->type;

        break;
    }
    return item1->type; // ??? WILL NOT GET THIS RETURN IF ALL IS GOOD
}
/**
 * Reduce
 *
 * @param parser_data Pointer to Parser_utils_t.
 * @return Returns 0 (OK) or Error code
 */
int reduce(Parser_utils_t *parser_data)
{

    int count = 0;
    bool found = 0;

    pt_rule resRule;

    Stack_item *item1 = NULL;
    Stack_item *item2 = NULL;
    Stack_item *item3 = NULL;

    // count symbols before stop sign

    Stack_item *tmp = s_top(stack);

    while (true)
    {

        if (tmp == NULL)
            break;

        if (tmp->symbol == STOP)
        {
            found = true;
            break;
        }

        else
            count++;

        tmp = tmp->nextItem;
    }

    if (found == 0)
    {
        return ERROR_SYNTAXE;
    }
    if (count == 1)
    {
        item1 = stack->top;
        item2 = NULL;
        item3 = NULL;
        resRule = test_rule(count, item1, item2, item3);
    }

    else if (count == 2)
    {

        item1 = stack->top;
        item2 = stack->top->nextItem;
        item3 = NULL;
        resRule = test_rule(count, item1, item2, item3);
    }

    else if (count == 3)
    {
        item1 = stack->top;
        item2 = stack->top->nextItem;
        item3 = stack->top->nextItem->nextItem;
        resRule = test_rule(count, item1, item2, item3);
    }

    if (resRule == ND_RULE)
    {
        fprintf(stderr, "Error code: %d! Expression: Not Define Rule.\n", ERROR_SYNTAXE);
        return ERROR_SYNTAXE;
    }
    if (resRule == E_EQU_E || resRule == E_LET_E || resRule == E_LTH_E || resRule == E_MET_E || resRule == E_MTH_E || resRule == E_NEQ_E)
    {

        if (parser_data->in_function)
        {
            if (parser_data->function->function_return == 1)
                return ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE;
        }

        if (parser_data->in_condition == 0)
        {
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
    }

    Data_type resType = test_semantics(item1, item2, item3, resRule);

    if (resType == ERROR_TYPE_UNCOMPATABILITY_ON_EXPR)
    {
        fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
        return resType;
    }

    for (int i = 0; i <= count; i++)
    {
        s_pop(stack);
    }
    // and push created non-terminal
    s_push(stack, resType, NON_TERM);

    return 0;
}
pt_rule test_rule(int count, Stack_item *item1, Stack_item *item2, Stack_item *item3)
{

    switch (count)
    {

    case 1:
        // E -> i
        if (IS_I(item1->symbol))
        {
            return E_RULE;
        }

        return ND_RULE;

    case 2:
        // E -> E!
        if (item1->symbol == NEG && item2->symbol == NON_TERM)
        {
            return E_NEG;
        }

        return ND_RULE;

    case 3:
        // E -> E operator E
        if (item1->symbol == NON_TERM && item3->symbol == NON_TERM)
        {
            // E -> E + E
            if (item2->symbol == PLUS)
                return E_PLUS_E;
            // E -> E - E
            else if (item2->symbol == MINUS)
                return E_MINUS_E;
            // E -> E * E
            else if (item2->symbol == MUL)
                return E_MUL_E;
            // E -> E / E
            else if (item2->symbol == DIV)
                return E_DIV_E;
            // E -> E < E
            else if (item2->symbol == LTN)
                return E_LTH_E;
            // E -> E <= E
            else if (item2->symbol == LEQ)
                return E_LET_E;
            // E -> E > E
            else if (item2->symbol == MTN)
                return E_MTH_E;
            // E -> E >= E
            else if (item2->symbol == MEQ)
                return E_MET_E;
            // E -> E == E
            else if (item2->symbol == EQ)
                return E_EQU_E;
            // E -> E != E
            else if (item2->symbol == NEQ)
                return E_NEQ_E;
            // E -> E ?? E
            else if (item2->symbol == Nil_Coalescing_Operator)
                return E_NIL_E;
        }

        else
        {
            // E -> ( E )
            if (item1->symbol == RIGHT_BRACKET && item2->symbol == NON_TERM && item3->symbol == LEFT_BRACKET)
                return LBR_E_RBR;
        }

        return ND_RULE;

    default:

        return ND_RULE;
    }
}
int rule_expression(Parser_utils_t *parser_data, FILE *file)
{
    int res = 0;
    s_init(stack);
    s_push(stack, (Data_type)DOLLAR, DOLLAR);
    pt_symbol firstTerminalSymbol;
    pt_symbol nextSymbol;
    Data_type nextType;

    pt_operation currentOperation;
    bool success = false;
    while (!success)
    {
        if (stack->top)
            firstTerminalSymbol = s_top_terminal_symbol(stack);
        nextSymbol = get_symbol_from_token(parser_data->parser_token);
        nextType = convert_token2type(parser_data);
        currentOperation = precedence_table[get_prec_table_index(firstTerminalSymbol)][get_prec_table_index(nextSymbol)];
        //
        switch (currentOperation)
        {
        case S:
            if ((res = shift(parser_data, stack, nextType, nextSymbol)))
                return res;
            if ((res = get_token(parser_data->parser_token, file)))
                return res;
            break;
        case R:
            if ((res = reduce(parser_data)))
            {
                return res;
            }
            break;
        case E:
            s_push(stack, nextType, nextSymbol);
            if ((res = get_token(parser_data->parser_token, file)))
                return res;

            break;
        case N:
            while (s_top_terminal_symbol(stack) != DOLLAR)
            {
                if ((res = reduce(parser_data)))
                    return res;
            }

            firstTerminalSymbol = s_top_terminal_symbol(stack);

            if ((nextSymbol == IDENTIFIER || nextSymbol == DOLLAR || nextSymbol == RIGHT_BRACKET) && firstTerminalSymbol == DOLLAR)
            {
                success = true;
                break;
            }
            else
            {
                if (!parser_data->parser_token->isNLINE)
                {
                    return ERROR_SYNTAXE;
                }
                else
                {
                    success = true;
                    break;
                }
            }

            break;
        }
    }
    Data_type resultTyp = s_top_type(stack);
    if (parser_data->in_function)
    {
        if (parser_data->function->function_return == 1)
        {
            if (resultTyp == TOKEN_INT || resultTyp == (Data_type)VAR_INT)
                resultTyp = (Data_type)IFJ_INT;
            else if (resultTyp == TOKEN_DOUBLE || resultTyp == (Data_type)VAR_DOUBLE)
                resultTyp = (Data_type)IFJ_DOUB;
            else if (resultTyp == TOKEN_STRING || resultTyp == (Data_type)VAR_STRING)
                resultTyp = (Data_type)IFJ_STR;
            else if (resultTyp == (Data_type)VAR_INT_NIL)
                resultTyp = (Data_type)IFJ_INT_NIL;
            else if (resultTyp == (Data_type)VAR_DOUBLE_NIL)
                resultTyp = (Data_type)IFJ_DOUB_NIL;
            else if (resultTyp == (Data_type)VAR_STRING_NIL)
                resultTyp = (Data_type)IFJ_STR_NIL;
            if (parser_data->function->type == IFJ_VOID)
            {
                if (resultTyp == (Data_type)DOLLAR)
                {
                    s_dispose(stack);
                    return 0;
                }
                else
                    return ERROR_BAD_EXPRESSION_ON_RETURN;
            }
            else if (parser_data->function->type != IFJ_VOID)
            {
                if (parser_data->function->type == (Data_type_IFJ)resultTyp)
                {
                    s_dispose(stack);
                    return 0;
                }
                else if (resultTyp == (Data_type)DOLLAR)
                    return ERROR_BAD_EXPRESSION_ON_RETURN;
                else
                {
                    fprintf(stderr, "EXPRESSION:ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE %d\n", ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE);
                    return ERROR_BAD_PARAM_OR_BAD_RETURN_TYPE;
                }
            }
        }
    }
    switch (resultTyp) // RESULT TYPE
    {
    case DOLLAR:
        return ERROR_SYNTAXE;
    case TOKEN_NIL:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
        {
            fprintf(stderr, "Error code: %d! Expression: Unknown Type.\n", ERROR_UNKNOWN_TYPE_VAR_OR_PARAM);
            return ERROR_UNKNOWN_TYPE_VAR_OR_PARAM;
        }
        if (parser_data->lhs->type != IFJ_INT_NIL && parser_data->lhs->type != IFJ_DOUB_NIL && parser_data->lhs->type != IFJ_STR_NIL && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    case TOKEN_INT:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
            parser_data->lhs->type = IFJ_INT;
        if (parser_data->lhs->type != IFJ_INT && parser_data->lhs->type != IFJ_INT_NIL && parser_data->lhs->type != IFJ_DOUB_NIL && parser_data->lhs->type != IFJ_DOUB && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    case VAR_INT:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
            parser_data->lhs->type = IFJ_INT;
        if (parser_data->lhs->type != IFJ_INT && parser_data->lhs->type != IFJ_INT_NIL && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    case TOKEN_DOUBLE:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
            parser_data->lhs->type = IFJ_DOUB;
        else if (parser_data->lhs->type != IFJ_DOUB && parser_data->lhs->type != IFJ_DOUB_NIL && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    case VAR_DOUBLE:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
            parser_data->lhs->type = IFJ_DOUB;
        else if (parser_data->lhs->type != IFJ_DOUB && parser_data->lhs->type != IFJ_DOUB_NIL && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    case VAR_STRING:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
        {
            parser_data->lhs->type = IFJ_STR;
        }
        else if (parser_data->lhs->type != IFJ_STR && parser_data->lhs->type != IFJ_STR_NIL && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    case TOKEN_STRING:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
        {
            parser_data->lhs->type = IFJ_STR;
        }
        else if (parser_data->lhs->type != IFJ_STR && parser_data->lhs->type != IFJ_STR_NIL && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    case VAR_INT_NIL:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
            parser_data->lhs->type = IFJ_INT_NIL;
        else if (parser_data->lhs->type != IFJ_INT_NIL && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    case VAR_DOUBLE_NIL:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
            parser_data->lhs->type = IFJ_DOUB_NIL;
        else if (parser_data->lhs->type != IFJ_DOUB_NIL && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    case VAR_STRING_NIL:
        if (parser_data->in_condition == 1)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        if (parser_data->lhs->type == IFJ_NOT_SET_TYPE)
            parser_data->lhs->type = IFJ_STR_NIL;
        else if (parser_data->lhs->type != IFJ_STR_NIL && parser_data->in_condition == 0)
        {
            fprintf(stderr, "Error code: %d! Expression: Uncompability of types.\n", ERROR_TYPE_UNCOMPATABILITY_ON_EXPR);
            return ERROR_TYPE_UNCOMPATABILITY_ON_EXPR;
        }
        break;
    default:

        break;
    }
    s_dispose(stack);
    return 0;
}