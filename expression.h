// Author: Vladyslav Malashchuk (xmalas04)
// project: IFJ2023 imperative language translator implementation
#include "symtable.h"
#include "parser.h"

#ifndef EXPRESSION_H
#define EXPRESSION_H
/**
 * @enum Precedence table symbols.
 */
#define IS_I(SYMBOL)						    \
	   SYMBOL == INT_NUMBER					    \
	|| SYMBOL == STRING					        \
	|| SYMBOL == NIL                            \
    || SYMBOL == IDENTIFIER						\
	|| SYMBOL == DOUBLE_NUMBER

typedef enum
{
	S, /// < SHIFT
	E, /// = EQUAL
	R, /// > REDUCE
	N  /// # ERROR
} pt_operation;
/**
 * @enum Indexes for precedence table.
 */
typedef enum
{
	I_PLUS_MINUS,			 /// 0 +-
	I_MUL_DIV,				 /// 1 */
	I_NEG,					 //  !
	I_NilCoalescingOperator, //??
	I_REL_OP,				 /// 3 r
	I_LEFT_BRACKET,			 /// 4 (
	I_RIGHT_BRACKET,		 /// 5 )
	I_DATA,					 /// 6 i
	I_DOLLAR				 /// 7 $
} pt_index;

/**
 * @enum Symbols used for Precedence analysis.
 */
typedef enum
{
	PLUS,			// +
	MINUS,			// -
	MUL,			// *
	DIV,			// /
	EQ,				// ==
	NEQ,			// !=
	LEQ,			// <=
	LTN,			// <
	MEQ,			// >=
	MTN,			// >
    NEG,            // !
    Nil_Coalescing_Operator, // ??
	LEFT_BRACKET,	/// (
	RIGHT_BRACKET,	// )
	IDENTIFIER,		// ID
	INT_NUMBER,		// int
	DOUBLE_NUMBER,	// double
	STRING,			// string
	NIL,
    DOLLAR,         // $
	STOP,			// stop symbol used when reducing
	NON_TERM,		// non-terminal
	VAR_INT,
	VAR_DOUBLE,
	VAR_STRING,
	VAR_INT_NIL,
	VAR_DOUBLE_NIL,
	VAR_STRING_NIL
} pt_symbol;

typedef enum {

    E_RULE,            // E -> i

	E_NEG,            // E -> ! E
    E_PLUS_E,         // E -> E + E
    E_MINUS_E,        // E -> E - E
    E_MUL_E,          // E -> E * E
    E_DIV_E,          // E -> E / E

    E_LTH_E,          // E -> E < E
    E_LET_E,          // E -> E <= E
    E_MTH_E,          // E -> E > E
    E_MET_E,          // E -> E >= E
    E_EQU_E,          // E -> E == E
    E_NEQ_E,          // E -> E != E
	
	E_NIL_E,          // E -> E ?? E

    LBR_E_RBR,        // E -> ( E )

    ND_RULE

} pt_rule;


/**
 * @brief Auxiliary function, converts token to data type
 * @param parserData contains token information
 * @return Data type
 */
Data_type convert_token2type( Parser_utils_t *parserData );

/**
 * @brief Auxiliary function, checks if token's value is zero
 * @param parserData contains token information
 * @return True if value is zero
 */
bool check_if_zero ( Parser_utils_t *parserData );

/**
 * @brief Applies <expression> rule
 * @param parserData contains all required information
 * @return Error code
 */
int rule_expression ( Parser_utils_t *parserData,FILE* file );



#endif // EXPRESSION_H