/*!
 * @file
 * @brief This file contains implementation of gpu
 *
 * @author Tomáš Milet, imilet@fit.vutbr.cz
 */

#include <student/gpu.hpp>
typedef struct Triangle {
	OutVertex points[3];
} primitive;
struct Point {
	float x, y, z;
} point;
void perspectiveDivision(primitive& triangle) {
	float xPos, yPos, zPos, wPos;
	for (int i = 0; i < 3; i++) {
		xPos = triangle.points[i].gl_Position.x;
		yPos = triangle.points[i].gl_Position.y;
		zPos = triangle.points[i].gl_Position.z;
		wPos = triangle.points[i].gl_Position.w;

		triangle.points[i].gl_Position.x = xPos / wPos;
		triangle.points[i].gl_Position.y = yPos / wPos;
		triangle.points[i].gl_Position.z = zPos / wPos;
	}
}
void viewportTransformation(primitive& triangle, uint32_t width, uint32_t height) {
	float xPos, yPos;
	for (int i = 0; i < 3; i++) {
		xPos = triangle.points[i].gl_Position.x;
		yPos = triangle.points[i].gl_Position.y;
		triangle.points[i].gl_Position.x = (float)width * ((xPos + 1) / 2);
		triangle.points[i].gl_Position.y = (float)height * ((yPos + 1) / 2);
	}
}
void readAttributes(GPUMemory& mem, InVertex& in, VertexArray& vao) {
	for (int i = 0; i < maxAttributes; i++) {
		const auto& attribute = vao.vertexAttrib[i];
		const void* buffer_ptr = mem.buffers[attribute.bufferID].data;
		//auto buffer = &buffer_ptr + attribute.offset + in.gl_VertexID * attribute.stride;
		void const* buffer = (uint8_t*)mem.buffers[vao.vertexAttrib[i].bufferID].data + in.gl_VertexID * vao.vertexAttrib[i].stride + vao.vertexAttrib[i].offset;
		switch (attribute.type) {
		case AttributeType::EMPTY:
			break;
		case AttributeType::FLOAT:
			in.attributes[i].v1 = *(float*)buffer;
			break;
		case AttributeType::VEC2:
			in.attributes[i].v2 = *(glm::vec2*)buffer;
			break;
		case AttributeType::VEC3:
			in.attributes[i].v3 = *(glm::vec3*)buffer;
			break;
		case AttributeType::VEC4:
			in.attributes[i].v4 = *(glm::vec4*)buffer;
			break;
		case AttributeType::UINT:
			in.attributes[i].u1 = *(uint32_t*)buffer;
			break;
		case AttributeType::UVEC2:
			in.attributes[i].u2 = *(glm::uvec2*)buffer;
			break;
		case AttributeType::UVEC3:
			in.attributes[i].u3 = *(glm::uvec3*)buffer;
			break;
		case AttributeType::UVEC4:
			in.attributes[i].u4 = *(glm::uvec4*)buffer;
			break;
		}
	}
}


uint32_t computeVertexID(GPUMemory& mem, VertexArray const& vao, uint32_t shaderInvocation) {
	if (vao.indexBufferID >= 0) {
		uint8_t* indexData = (uint8_t*)(mem.buffers[vao.indexBufferID].data) + vao.indexOffset;
		uint32_t index;
		switch (vao.indexType) {
		case IndexType::UINT8: { //if index type is uint8
			index = (uint32_t)(indexData[shaderInvocation]);
			break;
		}
		case IndexType::UINT16: { //if index type is uint16
			uint16_t* indexData16 = (uint16_t*)(indexData);
			index = (uint32_t)(indexData16[shaderInvocation]);
			break;
		}
		case IndexType::UINT32: { //if index type is uint32
			uint32_t* indexData32 = (uint32_t*)(indexData);
			index = indexData32[shaderInvocation];
			break;
		}
		}
		return index;
	}
	else {
		return shaderInvocation;
	}
}
void clear(GPUMemory& mem, ClearCommand cmd) {
	// výběr framebufferu
	Framebuffer* fbo = mem.framebuffers + mem.activatedFramebuffer;
	uint8_t* pixelStart = ((uint8_t*)fbo->color.data);
	float* pixelStartDepth = ((float*)fbo->depth.data);
	float red = cmd.color.r;
	float green = cmd.color.g;
	float blue = cmd.color.b;
	float alpha = cmd.color.a;
	if (cmd.clearColor) {
		if (fbo->color.data) {
			uint8_t* pixelStart = ((uint8_t*)fbo->color.data);
			for (uint32_t i = 0; i < fbo->width * fbo->height * fbo->color.channels; i += fbo->color.channels) {
				// Pokud jsou data typu float
				if (fbo->color.data == nullptr) continue;
				if (fbo->color.format == Image::FLOAT32) {
					float* pixelf = (float*)pixelStart;
					pixelf[i] = 255.f * red;
					pixelf[i + 1] = 255.f * green;
					pixelf[i + 2] = 255.f * blue;
					pixelf[i + 3] = 255.f * alpha;
				}
				// Pokud jsou data typu uint8_t
				if (fbo->color.format == Image::UINT8) {
					uint8_t* pixelu = (uint8_t*)pixelStart;
					pixelu[i] = uint8_t(255 * red);
					pixelu[i + 1] = uint8_t(255 * green);
					pixelu[i + 2] = uint8_t(255 * blue);
					pixelu[i + 3] = uint8_t(255 * alpha);
				}
			}
		}
	}
	if (cmd.clearDepth) {
		for (uint32_t i = 0; i < fbo->width * fbo->height; ++i) {
			if (fbo->depth.data == nullptr) continue;
			float* pixelf = (float*)pixelStartDepth;
			pixelf[i] = cmd.depth;
		}
	}
}
void runVertexAssembly(InVertex& inVertex, ShaderInterface& si, uint32_t shaderInvocationCounter, GPUMemory& mem) {
	inVertex.gl_VertexID = computeVertexID(mem, mem.vertexArrays[mem.activatedVertexArray], shaderInvocationCounter);
	si.gl_DrawID = mem.gl_DrawID;
	readAttributes(mem, inVertex, mem.vertexArrays[mem.activatedVertexArray]);
}
void primitiveAssembly(GPUMemory& mem, uint32_t trId/*, VertexArray& vao*/, VertexShader& vs, primitive& tr) {
	for (int i = 0; i < 3; i++) {
		InVertex in;
		OutVertex outVertex;
		ShaderInterface si;
		runVertexAssembly(in, si, trId * 3 + i, mem);
		si.textures = mem.textures;
		si.uniforms = mem.uniforms;
		vs(tr.points[i], in, si);
	}
}
void getEdges(primitive& primitive, glm::vec3 edges[]) {
	edges[0] = { primitive.points[1].gl_Position.x - primitive.points[0].gl_Position.x, primitive.points[1].gl_Position.y - primitive.points[0].gl_Position.y, 0 };
	edges[1] = { primitive.points[2].gl_Position.x - primitive.points[1].gl_Position.x, primitive.points[2].gl_Position.y - primitive.points[1].gl_Position.y, 0 };
	edges[2] = { primitive.points[0].gl_Position.x - primitive.points[2].gl_Position.x, primitive.points[0].gl_Position.y - primitive.points[2].gl_Position.y, 0 };
}
float crossProduct(glm::vec2 point, glm::vec3 p1, glm::vec3 p2) { return (point.x - p2.x) * (p1.y - p2.y) - (point.y - p2.y) * (p1.x - p2.x); }
bool isPixelInTriangle(primitive triangle, float pixel_x, float pixel_y) {
	glm::vec2 point = glm::vec2(pixel_x + 0., pixel_y);
	glm::vec3 a = triangle.points[0].gl_Position;
	glm::vec3 b = triangle.points[1].gl_Position;
	glm::vec3 c = triangle.points[2].gl_Position;

	float w1 = crossProduct(point, a, b);
	float w2 = crossProduct(point, b, c);
	float w3 = crossProduct(point, c, a);

	if ((w1 > 0 && w2 > 0 && w3 > 0 || w1 < 0 && w2 < 0 && w3 < 0) && (w1 != 0 && w2 != 0 && w3 != 0)) {
		return true;
	}

	return false;
}
bool counterClockWise(glm::vec3 edges[]) {
	if (glm::cross(edges[0], edges[1]).z >= 0 && glm::cross(edges[1], edges[2]).z >= 0 && glm::cross(edges[2], edges[0]).z >= 0)
		return true;
	else
		return false;
}
glm::vec3 inTriangleBarycentric(primitive& primitive, float x, float y) {
	glm::vec4 p0 = primitive.points[0].gl_Position;
	glm::vec4 p1 = primitive.points[1].gl_Position;
	glm::vec4 p2 = primitive.points[2].gl_Position;
	float lambda0 = ((p1.y - p2.y) * (x - p2.x) + (p2.x - p1.x) * (y - p2.y)) / ((p1.y - p2.y) * (p0.x - p2.x) + (p2.x - p1.x) * (p0.y - p2.y));
	float lambda1 = ((p2.y - p0.y) * (x - p2.x) + (p0.x - p2.x) * (y - p2.y)) / ((p1.y - p2.y) * (p0.x - p2.x) + (p2.x - p1.x) * (p0.y - p2.y));
	float lambda2 = 1.0f - lambda0 - lambda1;
	float z = p0.z * lambda0 + p1.z * lambda1 + p2.z * lambda2;
	return glm::vec3(lambda0, lambda1, lambda2);

}
void createFragment(GPUMemory& mem, primitive& primitive, InFragment& in, float x, float y) {
	glm::vec3 edges[3];
	getEdges(primitive, edges);
	in.gl_FragCoord = glm::vec4(x, y, 0, 0);
	auto deltas = inTriangleBarycentric(primitive, x, y);
	in.gl_FragCoord.z = deltas[0] * primitive.points[0].gl_Position.z + deltas[1] * primitive.points[1].gl_Position.z + deltas[2] * primitive.points[2].gl_Position.z;
	return;
}
void interpolateAttributes(primitive& primitive, Program& prg, InFragment& inFragment, float& deltaA, float& deltaB, float& deltaC) {

	// Calculate barycentric coordinates.
	float s = deltaA/primitive.points[0].gl_Position.w + deltaB / primitive.points[1].gl_Position.w + deltaC / primitive.points[2].gl_Position.w;
	float lambdaA = deltaA / (primitive.points[0].gl_Position.w*s);
	float lambdaB = deltaB / (primitive.points[1].gl_Position.w * s);
	float lambdaC = deltaC / (primitive.points[2].gl_Position.w * s);

	// Interpolate attributes.
	for (int i = 0; i < 4; i++) {
		switch (prg.vs2fs[i]) {
		case AttributeType::EMPTY:
			break;
		case AttributeType::FLOAT:
			inFragment.attributes[i].v1 = primitive.points[0].attributes[i].v1 * lambdaA + primitive.points[1].attributes[i].v1 * lambdaB + primitive.points[2].attributes[i].v1 * lambdaC;
			break;
		case AttributeType::VEC2:
			inFragment.attributes[i].v2 = primitive.points[0].attributes[i].v2 * lambdaA + primitive.points[1].attributes[i].v2 * lambdaB + primitive.points[2].attributes[i].v2 * lambdaC;
			break;
		case AttributeType::VEC3:
			inFragment.attributes[i].v3 = primitive.points[0].attributes[i].v3 * lambdaA + primitive.points[1].attributes[i].v3 * lambdaB + primitive.points[2].attributes[i].v3 * lambdaC;
			break;
		case AttributeType::VEC4:
			inFragment.attributes[i].v4 = primitive.points[0].attributes[i].v4 * lambdaA + primitive.points[1].attributes[i].v4 * lambdaB + primitive.points[2].attributes[i].v4 * lambdaC;
			break;
		}
	}

	// Calculate fragment depth.
	//float z0 = primitive.points[0].gl_Position.z;
	//float z1 = primitive.points[1].gl_Position.z;
	//float z2 = primitive.points[2].gl_Position.z;
	//inFragment.gl_FragCoord.z = z0 * lambdaA + z1 * lambdaB + z2 * lambdaC;
}
//float getFrameDepth(Framebuffer& frame, int x, int y) {
//	float depth = frame.depth[(y * frame.width + x)];
//	return depth;
//}
//int setFrameDepth(Framebuffer& frame, int x, int y, uint8_t D) {
//	frame.depth.data[(y * frame.width + x)] = 255;
//	return frame.depth.data[(y * frame.width + x)] = 255;
//}
//glm::vec4 setFrameColor(Framebuffer& frame, primitive triangle, GPUMemory& mem, int x, int y) {
//	uint8_t R, G, B, A;
//	mem.framebuffers[0].color[(y * frame.width + x) * 4] = R;
//	mem.framebuffers[0].color[(y * frame.width + x) * 4 + 1] = G;
//	mem.framebuffers[0].color[(y * frame.width + x) * 4 + 2] = B;
//	mem.framebuffers[0].color[(y * frame.width + x) * 4 + 3] = A;
//	return glm::vec4(R, G, B, A);
//}
bool compare_float(float x, float y) {
	float a = 0.1f;
	if (std::abs(x - y) < a)
		return true;
	else
		return false;
}
void clampColor(OutFragment& outFragment, float min, float max) {

	if (!compare_float(outFragment.gl_FragColor.x, 1.0) && !compare_float(outFragment.gl_FragColor.y, 1.0) && !compare_float(outFragment.gl_FragColor.z, 1.0))
		outFragment.gl_FragColor.x = std::max(std::min(outFragment.gl_FragColor.x, max), min);
	outFragment.gl_FragColor.x = outFragment.gl_FragColor.x * 255;

	outFragment.gl_FragColor.y = std::max(std::min(outFragment.gl_FragColor.y, max), min);
	outFragment.gl_FragColor.y = outFragment.gl_FragColor.y * 255;

	outFragment.gl_FragColor.z = std::max(std::min(outFragment.gl_FragColor.z, max), min);
	outFragment.gl_FragColor.z = outFragment.gl_FragColor.z * 255;
}
void perFragmentOperations(Framebuffer& frame, uint32_t x, uint32_t y, OutFragment& outFragment, float depth) {
	uint32_t pos = (frame.width) * y + x;
	uint8_t* Depth = (uint8_t*)frame.depth.data;
	uint8_t* Color = (uint8_t*)frame.color.data;
	if (Depth[pos] > depth) {
		if (outFragment.gl_FragColor.w > 0.5f) {
			Depth[pos] = depth;
		}
		///*(x * 4 + Color + 4 * y * frame.width) = ((float)*(x * 4 + Color + 4 * y * frame.width) / 255.f * (1.0f - outFragment.gl_FragColor[3]) + outFragment.gl_FragColor[0] * outFragment.gl_FragColor[3]) * 255.f;
		//*(x * 4 + 1 + Color + 4 * y * frame.width) = ((float)*(x * 4 + 1 + Color + 4 * y * frame.width) / 255.f * (1.0f - outFragment.gl_FragColor[3]) + outFragment.gl_FragColor[1] * outFragment.gl_FragColor[3]) * 255.f;
		//*(x * 4 + 2 + Color + 4 * y * frame.width) = ((float)*(x * 4 + 2 + Color + 4 * y * frame.width) / 255.f * (1.0f - outFragment.gl_FragColor[3]) + outFragment.gl_FragColor[2] * outFragment.gl_FragColor[3]) * 255.f;
		Color[4 * pos] = Color[4 * pos] * (1.0f - outFragment.gl_FragColor.w) + outFragment.gl_FragColor.x * outFragment.gl_FragColor.w;
		Color[4 * pos + 1] = Color[4 * pos + 1] * (1.0f - outFragment.gl_FragColor.w) + outFragment.gl_FragColor.y * outFragment.gl_FragColor.w;
		Color[4 * pos + 2] = Color[4 * pos + 2] * (1.0f - outFragment.gl_FragColor.w) + outFragment.gl_FragColor.z * outFragment.gl_FragColor.w;
	}
}
void rasterizeTriangle(DrawCommand& cmd, GPUMemory& mem, primitive& triangle, Program& prg) {
	glm::vec3 edges[3];
	getEdges(triangle, edges);
	for (int x = 0; x < mem.framebuffers[0].width; x++) {
		for (int y = 0; y < mem.framebuffers[0].height; y++) {
			float pointX = (float)x + 0.5f;
			float pointY = (float)y + 0.5f;
			if (isPixelInTriangle(triangle, pointX, pointY)) {
				if (!cmd.backfaceCulling || counterClockWise(edges)) {
					InFragment inFragment;
					OutFragment outFragment;
					Framebuffer frame = mem.framebuffers[0];

					createFragment(mem, triangle, inFragment, pointX, pointY);
					auto deltas = inTriangleBarycentric(triangle, pointX, pointY);
					interpolateAttributes(triangle, prg, inFragment, deltas[0], deltas[1], deltas[2]);
					//float depth = getFrameDepth(mem.framebuffers[0], x, y);
					//float fragDepth = setFrameDepth(frame, x, y, inFragment.gl_FragCoord.z);
					//auto fragColor = setFrameColor(frame, triangle, mem, x, y);
					//clampColor(outFragment, 0, 1);
					//perFragmentOperations(frame, pointX, pointY, outFragment, inFragment.gl_FragCoord.z);
					ShaderInterface si;
					//si.textures = mem.textures;
					//si.uniforms = mem.uniforms;

					prg.fragmentShader(outFragment, inFragment, si);
					//clampColor(outFragment, 0, 1);
					//perFragmentOperations(frame, x, y, outFragment, inFragment.gl_FragCoord.z);
				}
			}
		}
	}
}
void draw(GPUMemory& mem, DrawCommand cmd) {
	Program prg = mem.programs[mem.activatedProgram];
	VertexShader vs = mem.programs[mem.activatedProgram].vertexShader;
	uint32_t shaderInvocationCounter = 0;
	//ShaderInterface si;
	for (uint32_t n = 0; n < cmd.nofVertices / 3; ++n) {
		//InVertex inVertex;
		//OutVertex outVertex;
		//runVertexAssembly(inVertex, si, shaderInvocationCounter, mem);
		primitive tr;
		primitiveAssembly(mem, n, vs, tr);
		perspectiveDivision(tr);
		viewportTransformation(tr, mem.framebuffers[0].width, mem.framebuffers[0].height);
		rasterizeTriangle(cmd, mem, tr, prg);
		//(outVertex, inVertex, si);
		//shaderInvocationCounter++;
	}

}
//! [izg_enqueue]
void izg_enqueue(GPUMemory& mem, CommandBuffer const& cb) {
	(void)mem;
	(void)cb;
	/// \todo Tato funkce reprezentuje funkcionalitu grafické karty.<br>
	/// Měla by umět zpracovat command buffer, čistit framebuffer a kresli.<br>
	/// mem obsahuje paměť grafické karty.
	/// cb obsahuje command buffer pro zpracování.
	/// Bližší informace jsou uvedeny na hlavní stránce dokumentace.
	mem.gl_DrawID = 0;
	for (uint32_t i = 0; i < cb.nofCommands; ++i) {
		if (cb.commands[i].type == CommandType::BIND_FRAMEBUFFER) {
			// Set the active framebuffer in GPU memory
			mem.activatedFramebuffer = cb.commands[i].data.bindFramebufferCommand.id;
		}
		if (cb.commands[i].type == CommandType::BIND_PROGRAM) {
			// Set the active program in GPU memory
			mem.activatedProgram = cb.commands[i].data.bindProgramCommand.id;
		}
		if (cb.commands[i].type == CommandType::BIND_VERTEXARRAY) {
			// Set the active vertex array in GPU memory
			mem.activatedVertexArray = cb.commands[i].data.bindVertexArrayCommand.id;
		}
		if (cb.commands[i].type == CommandType::CLEAR) {
			clear(mem, cb.commands[i].data.clearCommand);
		}
		if (cb.commands[i].type == CommandType::DRAW) {
			// kresli

			DrawCommand cmd = cb.commands[i].data.drawCommand;
			draw(mem, cmd);
			// počítadlo kreslících příkazů
			mem.gl_DrawID++;

		}
		if (cb.commands[i].type == CommandType::SET_DRAW_ID) {
			mem.gl_DrawID = cb.commands[i].data.setDrawIdCommand.id;
		}
		if (cb.commands[i].type == CommandType::SUB_COMMAND) {
			izg_enqueue(mem, *cb.commands[i].data.subCommand.commandBuffer);
		}
	}
}
//! [izg_enqueue]

/**
 * @brief This function reads color from texture.
 *
 * @param texture texture
 * @param uv uv coordinates
 *
 * @return color 4 floats
 */
glm::vec4 read_texture(Texture const& texture, glm::vec2 uv) {
	if (!texture.img.data)return glm::vec4(0.f);
	auto& img = texture.img;
	auto uv1 = glm::fract(glm::fract(uv) + 1.f);
	auto uv2 = uv1 * glm::vec2(texture.width - 1, texture.height - 1) + 0.5f;
	auto pix = glm::uvec2(uv2);
	return texelFetch(texture, pix);
}

/**
 * @brief This function reads color from texture with clamping on the borders.
 *
 * @param texture texture
 * @param uv uv coordinates
 *
 * @return color 4 floats
 */
glm::vec4 read_textureClamp(Texture const& texture, glm::vec2 uv) {
	if (!texture.img.data)return glm::vec4(0.f);
	auto& img = texture.img;
	auto uv1 = glm::clamp(uv, 0.f, 1.f);
	auto uv2 = uv1 * glm::vec2(texture.width - 1, texture.height - 1) + 0.5f;
	auto pix = glm::uvec2(uv2);
	return texelFetch(texture, pix);
}

/**
 * @brief This function fetches color from texture.
 *
 * @param texture texture
 * @param pix integer coorinates
 *
 * @return color 4 floats
 */
glm::vec4 texelFetch(Texture const& texture, glm::uvec2 pix) {
	auto& img = texture.img;
	glm::vec4 color = glm::vec4(0.f, 0.f, 0.f, 1.f);
	if (pix.x >= texture.width || pix.y >= texture.height)return color;
	if (img.format == Image::UINT8) {
		auto colorPtr = (uint8_t*)getPixel(img, pix.x, pix.y);
		for (uint32_t c = 0; c < img.channels; ++c)
			color[c] = colorPtr[img.channelTypes[c]] / 255.f;
	}
	if (texture.img.format == Image::FLOAT32) {
		auto colorPtr = (float*)getPixel(img, pix.x, pix.y);
		for (uint32_t c = 0; c < img.channels; ++c)
			color[c] = colorPtr[img.channelTypes[c]];
	}
	return color;
}

