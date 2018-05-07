#pragma once

#include <QtWebEngineWidgets/QWebEngineView>
#include <QtWebChannel/QWebChannel>
#include <QtCore/QDebug>
#include <memory>
#include <type_traits>

namespace SSUtils
{
	namespace GUI
	{
		class QWebEngineWidget : public QWidget
		{
		public:
			explicit QWebEngineWidget(QWidget *parent = nullptr);
			~QWebEngineWidget(void) = default;

			inline QWebEngineView *view() { return m_view; }
			inline QWebChannel *channal() { return m_channal; }
			inline void load(const QString &url) { m_view->load(url); }
			inline void registerObject(const QString &name, QObject *obj) { m_channal->registerObject(name, obj); }
			template <typename T>
			inline T *registerObject(const QString &name, T *obj)
			{
				static_assert(!std::is_base_of_v<T, QObject>, "向WebChannal注册了非QObject子类的对象。");

				if (obj != nullptr)
				{
					registerObject(name, reinterpret_cast<QObject *>(obj));
				}
				return obj;
			}

			template <typename T>
			inline typename std::enable_if_t<std::is_base_of_v<QObject, T>, T> *registerObject(T *obj)
			{
				QString typeName(typeid(T).name());
				registerObject(typeName.right(typeName.size() - typeName.lastIndexOf("::") - 2), reinterpret_cast<QObject *>(obj));
				return obj;
			}

			template <typename T>
			inline typename std::enable_if_t<std::is_base_of_v<QObject, T>, typename std::shared_ptr<T>> registerObject(std::shared_ptr<T> &obj)
			{
				registerObject(obj.get());
				return obj;
			}

			template <typename T, typename... Args>
			inline typename std::enable_if_t<std::is_base_of_v<QObject, T>, typename std::shared_ptr<T>> registerObject(const QString &name, Args&&... args)
			{
				std::shared_ptr<T> obj(std::make_shared<T>(std::forward<Args>(args)...));
				registerObject(obj.get());
				return obj;
			}

			template <typename T, typename ...Args>
			inline typename std::enable_if_t<std::is_base_of_v<QObject, T>, typename std::shared_ptr<T>> registerObject(Args&&... args)
			{
				std::shared_ptr<T> obj(std::make_shared<T>(std::forward<Args>(args)...));
				registerObject(obj.get());
				return obj;
			}

		protected:
			void resizeEvent(QResizeEvent *e);

		protected:
			QWebEngineView * m_view;
			QWebChannel * m_channal;
		};
	};
};
