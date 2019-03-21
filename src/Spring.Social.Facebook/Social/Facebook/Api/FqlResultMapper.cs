﻿#region License

/*
 * Copyright 2011-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using System.IO;
using System.Collections.Generic;

namespace Spring.Social.Facebook.Api
{
	/// <summary>
	/// An interface used by FqlTemplate to map FQL results to objects of a specific type, on a per-object basis.
	/// Roughly analogous to a RowMapper used with Spring's JdbcTemplate.
	/// </summary>
	/// <author>Craig Walls</author>
	/// <author>SplendidCRM (.NET)</author>
	public interface FqlResultMapper<T>
	{
		//T mapObject(FqlResult objectValues);
	}
}
