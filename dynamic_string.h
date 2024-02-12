// Author: Vladyslav Malashchuk (xmalas04)
// project: IFJ2023 imperative language translator implementation
#ifndef _DYNAMIC_STRING_H
#define _DYNAMIC_STRING_H

#define DYNAMIC_STR_INC_MEM 8

#include <stdbool.h>
#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include "ifj_utils.h"

// Representation of dynamic string.
typedef struct 
{
    char *str; // DYNAMIC STRING
    unsigned int length; // Length of string
    unsigned int alloc_size; // Allocation Memory Size
} Dynamic_string_t;

/**
 * Inicialization of dynamic string.
 *
 * @param s Pointer on Dynamic String.
 * @return True inicialization OK, false NOT.
 */
int ds_init(Dynamic_string_t **s);

/**
 * Frees alocated memory.
 *
 * @param s Pointer on Dynamic String.
 */
void ds_free(Dynamic_string_t **s);

/**
 * Add char to the end of dynamic string.
 *
 * @param s Pointer on Dynamic String.
 * @param c Char to add.
 * @return True inicialization OK, false NOT.
 */
int ds_add_char(Dynamic_string_t *s, char c);

/**
 * Add constant string to the end of dynamic string.
 *
 * @param s Pointer on Dynamic String.
 * @param str Pointer on char.
 * @return True inicialization OK, false NOT.
 */
int ds_add_chars(Dynamic_string_t *s, char *str);


/**
 * Copy src string to dest string.
 *
 * @param src Pointer on Dynamic String SRC.
 * @param dest Pointer on Dynamic String DESC.
 * @return True inicialization OK, false NOT.
 */
int ds_copy(Dynamic_string_t *src, Dynamic_string_t *dest);

int ds_to_string(Dynamic_string_t *ds, char **str);

#endif