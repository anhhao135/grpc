/*
 *
 * Copyright 2015 gRPC authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

#ifndef GRPC_CORE_LIB_IOMGR_POLLSET_SET_H
#define GRPC_CORE_LIB_IOMGR_POLLSET_SET_H

#include "src/core/lib/iomgr/pollset.h"

#ifdef __cplusplus
extern "C" {
#endif

/* A grpc_pollset_set is a set of pollsets that are interested in an
   action. Adding a pollset to a pollset_set automatically adds any
   fd's (etc) that have been registered with the set_set to that pollset.
   Registering fd's automatically adds them to all current pollsets. */

typedef struct grpc_pollset_set grpc_pollset_set;

grpc_pollset_set *grpc_pollset_set_create(void);
void grpc_pollset_set_destroy(grpc_exec_ctx *exec_ctx,
                              grpc_pollset_set *pollset_set);
void grpc_pollset_set_add_pollset(grpc_exec_ctx *exec_ctx,
                                  grpc_pollset_set *pollset_set,
                                  grpc_pollset *pollset);
void grpc_pollset_set_del_pollset(grpc_exec_ctx *exec_ctx,
                                  grpc_pollset_set *pollset_set,
                                  grpc_pollset *pollset);
void grpc_pollset_set_add_pollset_set(grpc_exec_ctx *exec_ctx,
                                      grpc_pollset_set *bag,
                                      grpc_pollset_set *item);
void grpc_pollset_set_del_pollset_set(grpc_exec_ctx *exec_ctx,
                                      grpc_pollset_set *bag,
                                      grpc_pollset_set *item);

#ifdef __cplusplus
}
#endif

#endif /* GRPC_CORE_LIB_IOMGR_POLLSET_SET_H */