#pragma once

#include "QWebEngineWidget.h"
#include <QtCore/QObject>
#include <QtWidgets/QWidget>
#include <QtGui/QCloseEvent>

namespace SSUtils
{
	namespace GUI
	{
		class IWebEngineDialogWidget;
		template <typename T, typename U = std::enable_if_t<std::is_base_of_v<IWebEngineDialogWidget, T>>>
		class IWebEngineDialogInterface : public QObject
		{
		public:
			using DialogType = T;

		protected:
			explicit IWebEngineDialogInterface(T *dialog)
				: QObject(nullptr), m_dialog(dialog) {};

		public:
			IWebEngineDialogInterface(const IWebEngineDialogInterface &ano) = delete;
			IWebEngineDialogInterface(IWebEngineDialogInterface &&ano) = delete;
			IWebEngineDialogInterface &operator=(const IWebEngineDialogInterface &rhs) = delete;
			IWebEngineDialogInterface &operator=(IWebEngineDialogInterface &&rhs) = delete;
			virtual ~IWebEngineDialogInterface(void) = default;

		protected:
			T * m_dialog;
		};

		class IWebEngineDialogWidget : public QWebEngineWidget
		{
		public:
			explicit IWebEngineDialogWidget(const QString &guiEntrance, QWidget *parent = nullptr);
			IWebEngineDialogWidget(const IWebEngineDialogWidget &ano) = delete;
			IWebEngineDialogWidget(IWebEngineDialogWidget &&ano) = delete;
			IWebEngineDialogWidget &operator=(const IWebEngineDialogWidget &rhs) = delete;
			IWebEngineDialogWidget &operator=(IWebEngineDialogWidget &&rhs) = delete;
			virtual ~IWebEngineDialogWidget(void) = default;

		protected:
			virtual void initGUI(void) = 0;
			virtual void registerContents(void) = 0;

			void showEvent(QShowEvent *event);
			void closeEvent(QCloseEvent *event);

		private:
			void onLoadFinished(bool);

		protected:
			QString m_guiEntrance;
		};
	};
};
