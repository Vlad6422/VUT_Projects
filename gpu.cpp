/*!
 * @file
 * @brief This file contains implementation of gpu
 *
 * @author Tomáš Milet, imilet@fit.vutbr.cz
 */

#include <student/gpu.hpp>
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
void draw(GPUMemory& mem, DrawCommand cmd) {

	VertexShader vs = mem.programs[mem.activatedProgram].vertexShader;
	uint32_t shaderInvocationCounter = 0;
	ShaderInterface si;
	for (uint32_t n = 0; n < cmd.nofVertices; ++n) {
		InVertex inVertex;
		OutVertex outVertex;

		runVertexAssembly(inVertex, si, shaderInvocationCounter, mem);

		vs(outVertex, inVertex, si);
		shaderInvocationCounter++;
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

