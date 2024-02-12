// Author: Vladyslav Malashchuk (xmalas04)
// project: IFJ2023 imperative language translator implementation
#include "dynamic_string.h"
int ds_init(Dynamic_string_t **s)
{
    if(!(*s=(Dynamic_string_t*)malloc(sizeof(Dynamic_string_t)))) return ERROR_INTERNAL;
    if(!((*s)->str = malloc(DYNAMIC_STR_INC_MEM))){
        ds_free(s);
        return ERROR_INTERNAL;
    }
    (*s)->length = 0;
    (*s)->str[(*s)->length] = '\0';
    (*s)->alloc_size = DYNAMIC_STR_INC_MEM;

    return OK;
}

int ds_add_char(Dynamic_string_t *s, char c)
{
    if(s->length + 1 >= s->alloc_size)
    {
        s->alloc_size = s->length + DYNAMIC_STR_INC_MEM;
        if (!(s->str = (char *) realloc(s->str, s->alloc_size)))
		{
			return ERROR_INTERNAL;
		}
        
    } 

    s->str[s->length++] = c;
	s->str[s->length] = '\0';

	return OK;
    
}

int ds_add_chars(Dynamic_string_t *s, char *str)
{
    if(s->length + (unsigned int)strlen(str)+1 >= s->alloc_size)
    {
        s->alloc_size = s->length + strlen(str) + DYNAMIC_STR_INC_MEM;
        if (!(s->str = realloc (s->str,s->alloc_size))) return ERROR_INTERNAL;
    }
    strcat( s->str, str );
    s->length += strlen(str);
    s->str[s->length] = '\0';

    return OK;

}

int ds_copy(Dynamic_string_t *src, Dynamic_string_t *dest)
{
 if (src->length >= dest->alloc_size)
 {
    if (!(dest->str = realloc( dest->str, src->length + 1 ))) return ERROR_INTERNAL;
    dest->alloc_size = src->length + 1;
 }
    strcpy( dest->str, src->str );
    dest->length = src->length;
    
    return OK;
}

void ds_free(Dynamic_string_t **s)
{
    if(*s != NULL){
        if((*s)->str != NULL){
            free((*s)->str);
            (*s)->str=NULL;
        }
        free(*s);
        *s=NULL;
    }
}
int ds_to_string(Dynamic_string_t *ds, char **str) {
    if (ds == NULL || str == NULL) {
        return ERROR_INTERNAL; // Error: Invalid input
    }

    // Allocate memory for the new string
    *str = (char*)malloc(ds->length + 1);
    if (*str == NULL) {
        return ERROR_INTERNAL; // Memory allocation failed
    }

    strcpy(*str, ds->str);

    return OK; // Success
}