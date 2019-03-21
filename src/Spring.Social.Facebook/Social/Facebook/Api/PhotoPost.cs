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
using System.Collections.Generic;

namespace Spring.Social.Facebook.Api
{
	/// <summary>
	/// Model class representing a Post to a feed announcing a Photo. Note that this is not the Photo itself.
	/// To get the Photo object, get the Photo's ID by calling getPhotoId(), then calling getPhoto(photoId) on MediaOperations.
	/// </summary>
	/// <author>Craig Walls</author>
	/// <author>SplendidCRM (.NET)</author>
#if !SILVERLIGHT
	[Serializable]
#endif
	public class PhotoPost : Post
	{
		public PhotoPost()
		{
		}

		public PhotoPost(string id, Reference from, DateTime createdTime, DateTime updatedTime)
			: base(id, from, createdTime, updatedTime)
		{
		}
	
		public string PhotoId { get; set; }
	
		public List<Tag> Tags { get; set; }
	}
}
