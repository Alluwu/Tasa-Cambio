<template>
  <div class="app-layout">
    <header class="app-header">
      <h1>Tipo de Cambio</h1>
      <nav>
        <RouterLink to="/tasa-dia">Tasa del día</RouterLink>
        <RouterLink to="/tasa-rango">Rango</RouterLink>
        <RouterLink to="/bitacora">Bitácora</RouterLink>
      </nav>
    </header>

    <main class="app-main">
      <RouterView />
    </main>
  </div>
</template>
